using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurityValuation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurityValuation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuationController : ControllerBase
    {

        [HttpGet]
        public decimal PriceCalculator(string isin, DateTime valuationDate)
        {
   
            var bond = GetBondFromFile(isin);

            //calculate price based on bond elements
            double nominalPrice = 100;
            var interestRate = 0.03; //3%

            double cashFlow = (bond.CouponRate / nominalPrice) * 100; 

            var yearsElapsed = (bond.FirstPaymentDate.Date - valuationDate.Date).Days / 365.25;

            decimal disCashflowSum = 0;

            for(var i = bond.FirstPaymentDate.Year; i <= bond.MaturityDate.Year; i++)
            {
                if (i == bond.MaturityDate.Year)
                 cashFlow = cashFlow + nominalPrice;

                var amount = cashFlow / Math.Pow(1 + interestRate, yearsElapsed);                                   

                disCashflowSum = disCashflowSum + Convert.ToDecimal(amount);
                yearsElapsed++;
            }


            return decimal.Round(disCashflowSum, 2, MidpointRounding.AwayFromZero);
        }


        private BondDTO GetBondFromFile(string isin)
        {
            var bondLine = "";


            if (!string.IsNullOrEmpty(isin))
                bondLine = System.IO.File.ReadAllLines(@"..\SecurityValuation\Files\BondData.txt").FirstOrDefault(x => x.Contains(isin));

            BondDTO bond = new BondDTO();

            if (!string.IsNullOrEmpty(bondLine))
            {
                
                List<string> bondData = bondLine.Split(';').ToList();


                var couponRate = Regex.Matches(bondLine, @"(\d+).(\d+)+%");

                if (couponRate.Count < 1)
                    couponRate = Regex.Matches(bondLine, @"(\d+)+%");

                var cult = new CultureInfo("en-US");


                //TODO: check if any strings in bonddata are empty 

                bond = new BondDTO()
                {
                    InstrumentName = bondData[0],
                    ISIN = bondData[1],
                    MaturityDate = DateTime.ParseExact(bondData[2].Replace(".", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CouponFrequency = decimal.Parse(bondData[3], cult),
                    FirstPaymentDate = DateTime.ParseExact(bondData[4].Replace(".", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CouponRate = double.Parse(couponRate.First().Value.Replace("%", ""), cult)
                };
            }

            
            return bond;
        }
    }
}
