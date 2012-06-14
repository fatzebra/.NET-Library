using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FatZebra;

namespace FatZebra.Tests
{
    [TestClass]
    public class CreditCardsTest
    {

        [TestInitialize]
        public void Init()
        {
            FatZebra.Gateway.Username = "TEST";
            FatZebra.Gateway.Token = "TEST";
            Gateway.SandboxMode = true;
            Gateway.TestMode = true;
        }

        [TestMethod]
        public void TokenizedCardShouldBeSuccessful()
        {
            var response = CreditCard.Create("M SMith", "4005550000000001", DateTime.Now.AddYears(1), "123");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(((CreditCard)response.Result).ID);

            Assert.AreEqual(((CreditCard)response.Result).CardType, "VISA");
        }
    }
}
