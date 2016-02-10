using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class Purchase : IRequest
	{
		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("reference")]
		public string Reference { get; set; }

		[JsonProperty("customer_ip")]
		public string CustomerIP { get; set; }

		[JsonProperty("card_number")]
		public string CardNumber { get; set; }

		[JsonProperty("card_holder")]
		public string CardHolder { get; set; }

		[JsonProperty("card_expiry")]
		[JsonConverter(typeof(FatZebra.JsonConverters.ExpiryDateConverter))]
		public DateTime? CardExpiry { get; set; }

		[JsonProperty("cvv")]
		public string SecurityCode { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("test")]
		public Boolean TestMode { get; set; }

		[JsonProperty("fraud")]
		public FraudCheck FraudDetails { get; set; }

		[JsonProperty("card_token")]
		public string CardToken { get; set; }

		[JsonProperty("extra")]
		public Dictionary<String, Object> Extra { get; set; }
	}
}

