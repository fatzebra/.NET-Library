using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using FatZebra;
using Newtonsoft.Json;

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

		[Test]
		public void TokenizedCardWithAlphaNumericsShouldNotBeSuccessful()
		{
			var response = CreditCard.Create("Mark Smith", "wqwssasasasasasqwq5123 sasasasawqwq4567 wqsasaswq8901 wsasasasqwq2346", DateTime.Now.AddYears(1), "123");
			Assert.IsFalse(response.Successful);
			Assert.IsFalse(response.Result.Successful);
		}

		[Test]
		public void TokenizedCardWithSpacesShouldBeSuccessful()
		{
			var response = CreditCard.Create("Mark Smith", "5123 4567 8901 2346", DateTime.Now.AddYears(1), "123");

			Assert.IsTrue(response.Successful);
			Assert.IsTrue(response.Result.Successful);
		}

		[Test]
		public void TokenizedCardWithHyphensShouldBeSuccessful()
		{
			var response = CreditCard.Create("Mark Smith", "5123-4567-8901-2346", DateTime.Now.AddYears(1), "123");
			
			Assert.IsTrue(response.Successful);
			Assert.IsTrue(response.Result.Successful);
		}

		[Test]
		public void TokenizedCardFailingLuhnShouldNotBeSuccessful()
		{
			var response = CreditCard.Create("Mark Smith", "5123456789012347", DateTime.Now.AddYears(1), "123");
			
			Assert.IsFalse(response.Successful);
			Assert.IsFalse(response.Result.Successful);
		}

		[Test]
		public void FetchingCreditCardShouldBeSuccessful() 
		{
			var created_card = CreditCard.Create("Mark Smith", "5123-4567-8901-2346", DateTime.Now.AddYears(1), "123");
			var fetched = CreditCard.Find(created_card.Result.ID);

			Assert.IsNotNull(fetched);
		}

		[Test]
		public void FetchNonExistantCreditCardShouldBeNull()
		{
			var fetched = CreditCard.Find(Guid.NewGuid().ToString());

			Assert.IsNull(fetched);
		}
    }
}
