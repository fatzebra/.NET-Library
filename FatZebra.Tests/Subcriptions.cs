using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
    [TestFixture]
    public class Subcriptions
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

        [Test]
        public void TestSubscriptionFetchAllRecords()
        {
            var list = Subscription.All();

            Assert.AreNotEqual(list.Count, 0);
        }

        [Test]
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

        [Test]
        public void ShouldFindASubscription()
        {
            var plan_id = Guid.NewGuid().ToString();
            Plan.Create("testplan1", plan_id, "This is a test plan", 100);

            var customer_id = Guid.NewGuid().ToString();
            Customer.Create("Jim", "Smith", customer_id, "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));

            var sub_id = Guid.NewGuid().ToString();
            var subscription = Subscription.Create(customer_id, plan_id, "Weekly", sub_id, DateTime.Now.AddDays(1), true);

            var sub = ((Subscription)subscription.Result);

            var sub2 = Subscription.Find(sub.ID);
            Assert.AreEqual(sub2.ID, sub.ID);
        }
    }
}
