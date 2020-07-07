using System;
using System.Collections.Generic;
using System.Text;

namespace ExxchangeRateMonitorService
{
    public class ExxchangeRate
    {
        public string Base { get; set; }

        public DateTime Date { get; set; }

        public Currency Rates { get; set; }
    }

    public class Currency
    {
        public string INR { get; set; }

        public string SGD { get; set; }
    }
}
