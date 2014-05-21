using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class Customer : IRequest
	{
		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("reference")]
		public string Reference { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("ip_address")]
		public string CustomerIP { get; set; }

		[JsonProperty("card")]
		public Requests.CreditCard Card { get; set; }

		[JsonProperty("address")]
		public Requests.CustomerAddress Address { get; set; }

		[JsonProperty("test")]
		public bool TestMode { get; set; }

	}
}

