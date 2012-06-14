using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FatZebra.Tests
{
    [TestClass]
    public class Subcriptions
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
        public void SubscriptionShouldBeSuccessful()
        {
            var plan_id = Guid.NewGuid().ToString();
            var plan = Gateway.CreatePlan("testplan1", plan_id, "This is a test plan", 100);

            var customer_id = Guid.NewGuid().ToString();
            var customer = Gateway.CreateCustomer("Jim", "Smith", customer_id, "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));

            var sub_id = Guid.NewGuid().ToString();
            var subscription = Subscription.Create(customer_id, plan_id, "Weekly", sub_id, DateTime.Now.AddDays(1), true);

            Assert.IsTrue(subscription.Successful);
            Assert.IsNotNull(subscription.Result.ID);
            Assert.AreEqual(sub_id, ((Subscription)subscription.Result).Reference);
        }

        [TestMethod]
        public void TestSubscriptionFetchAllRecords()
        {
            var list = Subscription.All();

            Assert.AreNotEqual(list.Count, 0);
        }

        [TestMethod]
        public void ShouldCancelAndResumeASubscription()
        {
            var plan_id = Guid.NewGuid().ToString();
            var plan = Gateway.CreatePlan("testplan1", plan_id, "This is a test plan", 100);

            var customer_id = Guid.NewGuid().ToString();
            var customer = Gateway.CreateCustomer("Jim", "Smith", customer_id, "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));

            var sub_id = Guid.NewGuid().ToString();
            var subscription = Gateway.CreateSubscription(customer_id, plan_id, "Weekly", sub_id, DateTime.Now.AddDays(1), true);

            var sub = ((Subscription)subscription.Result);
            Assert.IsTrue(sub.IsActive);            
            sub.Cancel();
            Assert.IsFalse(sub.IsActive);
            sub.Resume();
            Assert.IsTrue(sub.IsActive);
        }
    }
}
