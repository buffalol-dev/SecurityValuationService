using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurityValuation.Models;
using SecurityValuation.Services;
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
        private readonly ValuationService _valuationService;
        public ValuationController(ValuationService valuationService)
        {
            _valuationService = valuationService;
        }

        [HttpGet]
        public double PriceCalculator(string isin, DateTime valuationDate)
        {

            return _valuationService.PriceCalculator(isin, valuationDate);
           
        }


    }
}
