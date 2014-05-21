using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class Subscription : IRequest
	{
		[JsonProperty("customer")]
		public string CustomerID { get; set; }

		[JsonProperty("plan")]
		public string PlanID { get; set; }

		[JsonProperty("frequency")]
		[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
		public SubscriptionFrequency Frequency { get; set; }

		[JsonProperty("start_date")]
		public DateTime StartDate { get; set; }

		[JsonProperty("end_date")]
		public DateTime? EndDate { get; set; }

		[JsonProperty("reference")]
		public string Reference { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("test_mode")]
		public bool TestMode { get; set; }

	}
}

