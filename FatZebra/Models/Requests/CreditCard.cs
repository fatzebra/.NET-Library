using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class CreditCard : IRequest
	{
		[JsonProperty("card_holder")]
		public string CardHolder { get; set; }

		[JsonProperty("card_number")]
		public string CardNumber { get; set; }

		[JsonProperty("card_expiry")]
		[JsonConverter(typeof(JsonConverters.ExpiryDateConverter))]
		public DateTime ExpiryDate { get; set; }

		// Temporarily alias card_expiry for customers until the API can be normalized
		[JsonProperty("expiry_date")]
		[JsonConverter(typeof(JsonConverters.ExpiryDateConverter))]
		public DateTime CardExpiry { get { return this.ExpiryDate; } }

		[JsonProperty("cvv")]
		public string SecurityCode { get; set; }

		[JsonProperty("test")]
		public bool TestMode { get; set; }
	}
}

