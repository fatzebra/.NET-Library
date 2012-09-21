using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
    [TestFixture]
    public class CreditCardsTest
    {

        [TestFixtureSetUp]
        public void Init()
        {
            FatZebra.Gateway.Username = "TEST";
            FatZebra.Gateway.Token = "TEST";
            Gateway.SandboxMode = true;
            Gateway.TestMode = true;
        }

        [Test]
        public void TokenizedCardShouldBeSuccessful()
        {
            var response = CreditCard.Create("M SMith", "4005550000000001", DateTime.Now.AddYears(1), "123");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(((CreditCard)response.Result).ID);

            Assert.AreEqual(((CreditCard)response.Result).CardType, "VISA");
        }

		[Test]
		public void TokenizedCardWithInvalidNumberShouldNotBeSuccessful()
		{
			var response = CreditCard.Create ("Mark Smith", "49927398716", DateTime.Now.AddYears(1), "123");
			Assert.IsFalse(response.Successful);
			Assert.IsFalse(response.Result.Successful);
		}
    }
}
