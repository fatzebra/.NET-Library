using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class Plan : IRequest
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("reference")]
		public string Reference { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("test")]
		public bool TestMode { get; set; }
	}
}

