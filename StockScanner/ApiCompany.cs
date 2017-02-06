using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScanner
{
    class ApiCompany
    {
        
        public string t { get; set; }
        public double l { get; set; }
        // Previous closing price
        public double pcls_fix { get; set; }
        // Change percentage
        public double cp { get; set; }
    }
}
