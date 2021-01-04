using SecurityValuation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurityValuation.Services
{
    public class ValuationService
    {
        public double PriceCalculator41(string isin, DateTime valuationDate)
        {
            var bond = GetBondFromFile(isin);

            double nominalPrice = 100;
            var interestRate = 0.03; //3%

            double cashFlow = (bond.CouponRate / nominalPrice) * 100;

            var yearsElapsed = (bond.FirstPaymentDate.Date - valuationDate.Date).Days / 365.25;

            decimal disCashflowSum = 0;

            for (var i = bond.FirstPaymentDate.Year; i <= bond.MaturityDate.Year; i++)
            {
                if (i == bond.MaturityDate.Year)
                    cashFlow = cashFlow + nominalPrice;

                var amount = cashFlow / Math.Pow(1 + interestRate, yearsElapsed);

                disCashflowSum = disCashflowSum + Convert.ToDecimal(amount);
                yearsElapsed++;
            }


            return Convert.ToDouble(disCashflowSum);

        }



        //single responsibility principle - possibly move this into its own service - security data retrieval and above method is part of this calculation class?
        private BondDTO GetBondFromFile(string isin)
        {
            var bondLine = "";


            if (!string.IsNullOrEmpty(isin))
                bondLine = System.IO.File.ReadAllLines(@"D:\Repos\SecurityValuation\SecurityValuation\Files\BondData.txt").FirstOrDefault(x => x.Contains(isin));

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
