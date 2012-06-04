using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FatZebra;

namespace FatZebra.Tests
{
    [TestClass]
    public class Gateway
    {
        private FatZebra.Gateway gw;

        [TestInitialize]
        public void Init()
        {
            gw = new FatZebra.Gateway("TEST", "TEST");
            gw.SandboxMode = true;
            gw.TestMode = true;
        }

        [TestMethod]
        public void PingShouldBeSuccessful()
        {
            Assert.IsTrue(gw.Ping());
        }

        [TestMethod]
        public void PurchaseShouldBeSuccessful()
        {
            var response = gw.Purchase(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 120);

            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.20);

            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
        }

        [TestMethod]
        public void PurchaseShouldReturnErrors()
        {
            var response = gw.Purchase(120, "M Smith", "", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsFalse(response.Successful);
            Assert.IsFalse(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 1);
        }

        [TestMethod]
        public void TokenizedCardShouldBeSuccessful()
        {
            var response = gw.TokenizeCard("M SMith", "4005550000000001", DateTime.Now.AddYears(1), "123");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(((CreditCard)response.Result).ID);

            Assert.AreEqual(((CreditCard)response.Result).CardType, "VISA");
        }

        [TestMethod]
        public void PurchaseWithTokenShouldBeSuccessful()
        {
            var card = gw.TokenizeCard("M SMith", "5123456789012346", DateTime.Now.AddYears(1), "123");
            Assert.IsTrue(card.Successful);
            var response = gw.Purchase(123, card.Result.ID, "123", Guid.NewGuid().ToString(), "123.123.123.1");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 0);
            Assert.AreEqual(((Purchase)response.Result).Amount, 123);

            Assert.AreEqual(((Purchase)response.Result).DecimalAmount, 1.23);

            Assert.AreEqual(((Purchase)response.Result).CardType, "MasterCard");
        }

        [TestMethod]
        public void RefundShouldBeSuccessful()
        {
            var purchase = gw.Purchase(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");

            var refund = gw.Refund(120, purchase.Result.ID, "Refund" + Guid.NewGuid().ToString());

            Assert.IsTrue(refund.Successful);
            Assert.IsTrue(refund.Result.Successful);
            Assert.IsNotNull(refund.Result.ID);
            Assert.AreEqual(((Refund)refund.Result).Amount, -120);
        }

        [TestMethod]
        public void PlansShouldBeSuccessful()
        {
            var plan = gw.CreatePlan("testplan1", "tp1", "This is a test plan", 100);
            Assert.IsTrue(plan.Successful);
            Assert.IsNotNull(plan.Result.ID);
            Assert.AreEqual(((Plan)plan.Result).Amount, 100);
        }

        [TestMethod]
        public void CustomerShouldBeSuccessful()
        {
            var customer = gw.CreateCusotmer("Jim", "Smith", Guid.NewGuid().ToString(), "jim@smith.com", "5123456789012346", "Jim Smith", "123", DateTime.Now.AddYears(1));
            Assert.IsTrue(customer.Successful);
            Assert.IsNotNull(customer.Result.ID);

            Assert.AreEqual("Jim Smith", ((Customer)customer.Result).CustomerName);
        }
    }
}
