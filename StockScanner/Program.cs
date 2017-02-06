using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Script.Serialization;

namespace StockScanner
{
    class Program
    {
        private static JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        static void Main(string[] args)
        {
            Dictionary<string, Tuple<bool, bool>> previousPredictions = new Dictionary<string, Tuple<bool, bool>>();
            Dictionary<string, bool> predictions = new Dictionary<string, bool>();

            using (StockTrackerEntities db = new StockTrackerEntities())
            {
                foreach (Company company in db.Companies)
                {
                    ApiCompany apiCompany = GetStockPrice(company.symbol);

                    // Change the influence of the keywords that were in yesterday's headlines
                    foreach (HeadlineKeyword keyword in db.HeadlineKeywords.Where(keyword => keyword.symbol == company.symbol))
                    {
                        KeywordInfluence existingKeyword = db.KeywordInfluences.FirstOrDefault(k => k.industry_type == company.industry_type && k.keyword == keyword.keyword);
                        if (existingKeyword == null)
                        {
                            existingKeyword = db.KeywordInfluences.Add(new KeywordInfluence()
                            {
                                Industry = company.Industry,
                                keyword = keyword.keyword,
                                influence = 0,
                                influence_counter = 0
                            });
                        }
                        existingKeyword.influence = ((existingKeyword.influence * existingKeyword.influence_counter) + apiCompany.cp) / ++existingKeyword.influence_counter;
                    }
                    company.price = apiCompany.l;

                    // What did we think the outcome would be?
                    Prediction predictedOutcome = db.Predictions.FirstOrDefault(p => p.Company.symbol == company.symbol);
                    if (predictedOutcome != null)
                    {
                        previousPredictions.Add(company.symbol, new Tuple<bool, bool>(predictedOutcome.predicted_change == (apiCompany.pcls_fix > 0), predictedOutcome.predicted_change));
                    }
                }

                // Clear the headline keywords for EV-ERY-THING
                db.HeadlineKeywords.RemoveRange(db.HeadlineKeywords);
                db.Predictions.RemoveRange(db.Predictions);

                // Send out the email telling me the results of the predictions.
                SendEmailToSelf("Stock Prediction Results", "The following are the results of the stock predictions:\n" +
                    string.Concat(previousPredictions.Select(p =>
                        string.Format("{0}: {1}. Prediction was {2}.\n", p.Key, (p.Value.Item1 ? "CORRECT" : "INCORRECT"), (p.Value.Item2 ? "UP" : "DOWN")))));

                db.SaveChanges();

                foreach (Company company in db.Companies)
                {
                    NYTimesResults searchResults = GetNYTimesHeadlines(company.name);
                    
                    Prediction prediction = new Prediction()
                    {
                        Company = company
                    };

                    double influence = 0;
                    foreach (string headlineKeyword in searchResults.response.docs.SelectMany(d =>
                        d.headline.main.Replace(':', ' ').Replace('/', ' ').Replace(".", "").Split(' ')))
                    {

                        // Is the keyword blacklisted or an integer?
                        int possibleInt;
                        if (db.BlacklistedKeywords.Any(blk =>
                            blk.keyword == headlineKeyword &&
                            (blk.industry_type == company.industry_type || blk.industry_type == 0)) 
                            || int.TryParse(headlineKeyword, out possibleInt))
                        {
                            continue;
                        }
                        db.HeadlineKeywords.Add(new HeadlineKeyword()
                        {
                            Company = company,
                            keyword = headlineKeyword
                        });

                        // Is the keyword in the existing influence table? If so, change the prediction.
                        KeywordInfluence existingKeyword = db.KeywordInfluences.FirstOrDefault(keyword =>
                            keyword.industry_type == company.industry_type &&
                            keyword.keyword == headlineKeyword);

                        if (existingKeyword != null)
                        {
                            influence += existingKeyword.influence;
                        }
                    }

                    prediction.predicted_change = influence > 0;
                    db.Predictions.Add(prediction);
                    db.SaveChanges();
                }

                SendEmailToSelf("Stock Predictions", "The following are the predictions for the stocks:\n" +
                    string.Concat(db.Predictions.Select(p => p.Company.name + ": " + (p.predicted_change ? "UP" : "DOWN") + "\n")));
            }
        }

        private static NYTimesResults GetNYTimesHeadlines(string companyName)
        {
            string nyTimesAPIKey = System.Configuration.ConfigurationManager.AppSettings["NyTimesApiKey"];
            string nyTimesUrl = string.Format(@"http://api.nytimes.com/svc/search/v2/articlesearch.json?q={0}&fq=headline%3A{1}&begin_date=20160101&end_date=20160102&fl=headline&api-key={2}", companyName, companyName, nyTimesAPIKey);
            WebRequest request = WebRequest.Create(nyTimesUrl);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return jsSerializer.Deserialize<NYTimesResults>(reader.ReadToEnd());
                }
            }
        }

        private static ApiCompany GetStockPrice(string symbol)
        {
            // Fetch the company's current price and previous price.
            WebRequest request = WebRequest.Create("http://www.google.com/finance/info?q=NSE:" + symbol);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd().Replace("//", "");
                    return (jsSerializer.Deserialize<ApiCompany[]>(json))[0];
                }
            }
        }

        private static void SendEmailToSelf(string subject, string body)
        {
            string emailAddress = System.Configuration.ConfigurationManager.AppSettings["EmailAddress"];
            string emailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
            SmtpClient client = new SmtpClient(emailHost, 587)
            {
                Credentials = new NetworkCredential(emailAddress, emailPassword),
                EnableSsl = true
            };
            client.Send(emailAddress, emailAddress, subject, body);
        }
    }
}
