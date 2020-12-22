using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityValuation.Controllers;
using System;

namespace SecurityValuation.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            var valuationController = new ValuationController();

            //act
            var result = valuationController.PriceCalculator("XS1849479602", new DateTime(2020,11,28));

            //assert
            Assert.AreEqual(126.2256, result, 0.001);
            
        }
    }
}
