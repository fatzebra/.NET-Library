using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
    [TestFixture]
    public class Plans
    {
        [OneTimeSetUp]
        public void Init()
        {
            FatZebra.Gateway.Username = "TEST";
            FatZebra.Gateway.Token = "TEST";
            Gateway.SandboxMode = true;
            Gateway.TestMode = true;
        }

        [Test]
        public void PlansShouldBeSuccessful()
        {
            var plan_id = Guid.NewGuid().ToString();
            var plan = Plan.Create("testplan1", plan_id, "This is a test plan", 100);
            Assert.IsTrue(plan.Successful);
            Assert.IsNotNull(plan.Result.ID);
            Assert.AreEqual(((Plan)plan.Result).Amount, 100);
        }

        [Test]
        public void ShouldFindAPlan()
        {
            var plan_id = Guid.NewGuid().ToString();
            var plan = Plan.Create("testplan1", plan_id, "This is a test plan", 100);

            var thePlan = ((Plan)plan.Result);

            var plan2 = Plan.Find(thePlan.ID);
            Assert.AreEqual(plan2.ID, thePlan.ID);
        }
    }
}
