using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class Refund : IRequest
	{
		[JsonProperty("transaction_id")]
		public string TransactionID { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set;}

		[JsonProperty("reference")]
		public string Reference { get; set;}

		[JsonProperty("test")]
		public bool TestMode { get; set; }
	}
}

