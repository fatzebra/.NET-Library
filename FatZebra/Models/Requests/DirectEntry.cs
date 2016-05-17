using System;
using Newtonsoft.Json;

namespace FatZebra.Requests
{
	public class DirectEntry : IRequest
	{
		/// <summary>
		/// The customers account name
		/// </summary>
		[JsonProperty("account_name")]
		public string AccountName { get; set; }
		/// <summary>
		/// The customers account number
		/// </summary>
		[JsonProperty("account_number")]
		public string AccountNumber { get; set; }
		/// <summary>
		/// The customers account BSB
		/// </summary>
		[JsonProperty("bsb")]
		public string BSB { get; set; }
		/// <summary>
		/// The amount of the entry
		/// </summary>
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// The reference for the entry
		/// </summary>
		/// <value>The reference.</value>
		[JsonProperty("reference")]
		public string Reference { get; set; }
		/// <summary>
		/// The description for the entry - this is displayed on the customers account statement
		/// and is limited to 18 characters
		/// </summary>
		/// <value>The description.</value>
		[JsonProperty("description")]
		public string Description { get; set; }
		/// <summary>
		/// The date the DE should be processed on
		/// </summary>
		/// <value>The date.</value>
		[JsonProperty("date")]
		public DateTime Date { get; set; }

		[JsonProperty("test")]
		public bool TestMode { get; set; }
	}
}

