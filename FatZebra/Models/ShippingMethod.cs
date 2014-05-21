using System;
using Newtonsoft.Json;

namespace FatZebra
{
	public enum ShippingMethod
	{
		[JsonProperty("low_cost")]
		LowCost,
		[JsonProperty("same_day")]
		SameDay,
		[JsonProperty("overnight")]
		Overnight,
		[JsonProperty("express")]
		Express,
		[JsonProperty("international")]
		International,
		[JsonProperty("pickup")]
		Pickup,
		[JsonProperty("other")]
		Other
	}
}

