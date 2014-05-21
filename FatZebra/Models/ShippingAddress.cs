using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FatZebra
{
	public class ShippingAddress
	{
		/// <summary>
		/// The shipping address first name (usually the customers first name).
		/// </summary>
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		/// <summary>
		/// The shipping address last name (usually the customers last name).
		/// </summary>
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		/// <summary>
		/// The shipping email address (usually the customers).
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }
		/// <summary>
		/// The shipping primary address line
		/// </summary>
		[JsonProperty("address_1")]
		public string AddressLine1 { get; set; }
		/// <summary>
		/// The shipping secondary address line
		/// </summary>
		[JsonProperty("address_2")]
		public string AddressLine2 { get; set; }
		/// <summary>
		/// The shipping city
		/// </summary>
		[JsonProperty("city")]
		public string City { get; set; }
		/// <summary>
		/// The shipping postal code
		/// </summary>
		[JsonProperty("post_code")]
		public string PostCode { get; set; }
		/// <summary>
		/// The shipping country
		/// </summary>
		[JsonProperty("country")]
		public string Country { get; set; }
		/// <summary>
		/// The shipping home or primary (e.g. cellular) phone number
		/// </summary>
		[JsonProperty("home_phone")]
		public string HomePhone { get; set;}
		/// <summary>
		/// The shipping secondary or work phone number
		/// </summary>
		[JsonProperty("work_phone")]
		public string WorkPhone { get; set; }
		/// <summary>
		/// The shipping method for this order
		/// </summary>
		[JsonProperty("shipping_method")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ShippingMethod ShipMethod { get; set; }
	}
}

