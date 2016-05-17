using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
	public abstract class DirectEntry : IRecord
	{
		/// <summary>
		/// The record ID
		/// </summary>
		/// <value>The I.</value>
		[JsonProperty("id")]
		public string ID { get; set; }
		[JsonProperty("successful")]
		public bool Successful { 
			get {
				return true;
			}
		}
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
		/// <summary>
		/// The date the record has been processed.
		/// </summary>
		/// <value>The process date.</value>
		[JsonProperty("process_date")]
		public DateTime? ProcessDate { get; set; }
	
		/// <summary>
		/// The status of the DE record
		/// </summary>
		[JsonProperty("status")]
		[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
		public DirectEntryStatus Status { get; set; }

		/// <summary>
		/// The result of the DE record
		/// </summary>
		/// <value>The result.</value>
		[JsonProperty("result")]
		public string Result { get; set; }

        public abstract bool Delete();
	}

	public enum DirectEntryStatus {
		New,
		Pending,
		Completed,
		Rejected,
		Delete
	}
}
