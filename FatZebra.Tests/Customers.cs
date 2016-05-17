using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;

namespace FatZebraTests.Tests
{
    [TestFixture]
    public class Customers
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
        public void CustomerShouldBeSuccessful()
        {
            var customer = Customer.Create("Jim", "Smith", Guid.NewGuid().ToString(), "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));
            Assert.IsTrue(customer.Successful);
            Assert.IsNotNull(customer.Result.ID);

			Assert.AreEqual("Jim Smith", customer.Result.CustomerName);
        }

        [Test]
        public void ShouldFindACustomer()
        {
            var customer = Customer.Create("Jim", "Smith", Guid.NewGuid().ToString(), "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));

            var cust1 = ((Customer)customer.Result);
            var cust2 = Customer.Find(cust1.ID);

            Assert.AreEqual(cust1.CustomerName, cust2.CustomerName);
        }

        [Test]
        public void ShouldUpdateACustomersCard()
        {
            var customer = Customer.Create("Jim", "Smith", Guid.NewGuid().ToString(), "jim@smith.com", "Jim Smith", "5123456789012346", "123", DateTime.Now.AddYears(1));
            var cust = customer.Result;

			Assert.IsTrue(cust.UpdateCard("Wally Smith", "4444333322221111", DateTime.Now.AddYears(1), "123"));
        }
    }
}
