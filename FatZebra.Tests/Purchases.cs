using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
    [TestFixture]
    public class Purchases
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
        public void PurchaseShouldBeSuccessful()
        {
            var response = Purchase.Create(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 120);

            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.20);

            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
        }

        [Test]
        public void PurchaseShouldReturnErrors()
        {
            var response = Purchase.Create(120, "M Smith", "", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsFalse(response.Successful);
            Assert.IsFalse(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 1);
        }

        [Test]
        public void PurchaseWithTokenShouldBeSuccessful()
        {
            var card = CreditCard.Create("M SMith", "5123456789012346", DateTime.Now.AddYears(1), "123");
            Assert.IsTrue(card.Successful);
            var response = Purchase.Create(123, card.Result.ID, "123", Guid.NewGuid().ToString(), "123.123.123.1");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 123);

            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.23);

            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
        }
    }
}
