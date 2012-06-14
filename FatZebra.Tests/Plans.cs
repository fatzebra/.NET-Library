using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FatZebra.Tests
{
    [TestClass]
    public class Plans
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
        public void PlansShouldBeSuccessful()
        {
            var plan_id = Guid.NewGuid().ToString();
            var plan = Plan.Create("testplan1", plan_id, "This is a test plan", 100);
            Assert.IsTrue(plan.Successful);
            Assert.IsNotNull(plan.Result.ID);
            Assert.AreEqual(((Plan)plan.Result).Amount, 100);
        }

        [TestMethod]
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
