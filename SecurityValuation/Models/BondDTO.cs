using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityValuation.Models
{
    public class BondDTO
    {
        public decimal CouponFrequency { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public string ISIN { get; set; }
        public string InstrumentName { get; set; }
        public double CouponRate { get; set; }
    }
}
