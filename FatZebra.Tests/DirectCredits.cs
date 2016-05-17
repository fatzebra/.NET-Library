using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;
using Newtonsoft.Json;

namespace FatZebra.Tests
{
    [TestFixture]
    public class DirectCreditsTest
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
        public void NewDirectCreditShouldBeSuccessful()
        {
            var response = DirectCredit.Create("012-084", "123123123", "Max Smith", 123.00m, Guid.NewGuid().ToString(), "DotNet DE", DateTime.Today);

            Assert.IsTrue(response.Successful);
            Assert.IsTrue(response.Result.Successful);
            Assert.IsNotNull(((DirectCredit)response.Result).ID);

            Assert.AreEqual(((DirectCredit)response.Result).BSB, "012-084");
            Assert.IsTrue(((DirectCredit)response.Result).ID.Contains("-DC-"));
        }

        [Test]
        public void FetchDirectCreditShouldBeSuccessful()
        {
            var response1 = DirectCredit.Create("012-084", "123123123", "Max Smith", 123.00m, Guid.NewGuid().ToString(), "DotNet DE", DateTime.Today);
            var response2 = DirectCredit.Find(response1.Result.ID);

            Assert.IsNotNull(response2);
            Assert.AreEqual(response1.Result.AccountName, response2.AccountName);
        }

        [Test]
        public void DeleteDirectCreditShouldBeSuccessful()
        {
            var response1 = DirectCredit.Create("012-084", "123123123", "Max Smith", 123.00m, Guid.NewGuid().ToString(), "DotNet DE", DateTime.Today);

            Assert.IsTrue(response1.Result.Delete());
        }
    }
}
