using System;
using Newtonsoft.Json;

namespace FatZebra
{
	public class OrderCustomer
	{
		/// <summary>
		/// The customers record ID in the merchants system
		/// </summary>
		[JsonProperty("id")]
		public string ID { get; set; }
		/// <summary>
		/// The customers First Name
		/// </summary>
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		/// <summary>
		/// The customers Last Name
		/// </summary>
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		/// <summary>
		/// The customers email address
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }
		/// <summary>
		/// The customers Date Of Birth
		/// </summary>
		[JsonProperty("date_of_birth")]
		public DateTime DOB { get; set; }
		/// <summary>
		/// The customers Address (Line 1)
		/// </summary>
		[JsonProperty("address_1")]
		public string AddressLine1 { get; set; }
		/// <summary>
		/// The customers address (Line 2, Secondary)
		/// </summary>
		[JsonProperty("address_2")]
		public string AddressLine2 { get; set; }
		/// <summary>
		/// The customers city/suburb.
		/// </summary>
		[JsonProperty("city")]
		public string City { get; set; }
		/// <summary>
		/// The customers postal code
		/// </summary>
		[JsonProperty("post_code")]
		public string PostCode { get; set; }
		/// <summary>
		/// The customers country
		/// </summary>
		[JsonProperty("country")]
		public string Country { get; set; }
		/// <summary>
		/// The customers home or primary phone number
		/// </summary>
		[JsonProperty("home_phone")]
		public string HomePhone { get; set; }
		/// <summary>
		/// The customers word or secondary phone number
		/// </summary>
		[JsonProperty("work_phone")]
		public string WorkPhone { get; set; }
		/// <summary>
		/// Indicates if the customers already exists in the merchants system
		/// </summary>
		[JsonProperty("existing_customer")]
		public bool ExistingCustomer { get; set; }
		/// <summary>
		/// The timestamp the customer record was created at, used to calculate the Time on File
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }
	}
}

