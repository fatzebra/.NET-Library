using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FatZebra;

namespace FatZebra.Tests
{
    [TestClass]
    public class GatewayTest
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
        public void PingShouldBeSuccessful()
        {
            Assert.IsTrue(Gateway.Ping());
        }

        [TestMethod]
        public void PurchaseShouldBeSuccessful()
        {
            var response = Gateway.Purchase(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
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
            var response = Gateway.Purchase(120, "M Smith", "", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");
            Assert.IsFalse(response.Successful);
            Assert.IsFalse(response.Result.Successful);
            Assert.IsNotNull(response.Result.ID);
            Assert.AreEqual(response.Errors.Count, 1);
        }

        [TestMethod]
        public void TokenizedCardShouldBeSuccessful()
        {
            var response = Gateway.TokenizeCard("M SMith", "4005550000000001", DateTime.Now.AddYears(1), "123");

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(((CreditCard)response.Result).ID);

            Assert.AreEqual(((CreditCard)response.Result).CardType, "VISA");
        }

        [TestMethod]
        public void PurchaseWithTokenShouldBeSuccessful()
        {
            var card = Gateway.TokenizeCard("M SMith", "5123456789012346", DateTime.Now.AddYears(1), "123");
            Assert.IsTrue(card.Successful);
            var response = Gateway.Purchase(123, card.Result.ID, "123", Guid.NewGuid().ToString(), "123.123.123.1");

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
            var purchase = Gateway.Purchase(120, "M Smith", "5123456789012346", DateTime.Now.AddYears(1), "123", Guid.NewGuid().ToString(), "123.0.0.1");

            var refund = Gateway.Refund(120, purchase.Result.ID, "Refund" + Guid.NewGuid().ToString());

            Assert.IsTrue(refund.Successful);
            Assert.IsTrue(refund.Result.Successful);
            Assert.IsNotNull(refund.Result.ID);
            Assert.AreEqual(((Refund)refund.Result).Amount, -120);
        }

        [TestMethod]
        public void PlansShouldBeSuccessful()
        {
            var plan_id = Guid.NewGuid().ToString();
            var plan = Gateway.CreatePlan("testplan1", plan_id, "This is a test plan", 100);
            Assert.IsTrue(plan.Successful);
            Assert.IsNotNull(plan.Result.ID);
            Assert.AreEqual(((Plan)plan.Result).Amount, 100);
        }

        [TestMethod]
        public void CustomerShouldBeSuccessful()
        {
            var customer = Gateway.CreateCustomer("Jim", "Smith", Guid.NewGuid().ToString(), "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));
            Assert.IsTrue(customer.Successful);
            Assert.IsNotNull(customer.Result.ID);

            Assert.AreEqual("Jim Smith", ((Customer)customer.Result).CustomerName);
        }
    }
}
