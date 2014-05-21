using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class CustomerAddress : IRequest
	{
		[JsonProperty("address")]
		public string Address { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("postcode")]
		public string PostCode { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }
	}
}

