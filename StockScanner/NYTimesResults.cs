using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScanner
{
    class NYTimesResults
    {
        public JsonResponse response { get; set; }
        public string status { get; set; }
    }

    class JsonResponse
    {
        public Doc[] docs { get; set; }
    }

    class Doc
    {
        public Headline headline { get; set; }
    }

    class Headline
    {
        public string main { get; set; }
    }
}
