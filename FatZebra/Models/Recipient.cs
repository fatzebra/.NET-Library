using System;
using Newtonsoft.Json;

namespace FatZebra
{
	public class Recipient
	{
		/// <summary>
		/// The recipients title/saultation (e.g. Mr, Mrs, Miss, Dr)
		/// </summary>
		[JsonProperty("title")]
		public string Title { get; set; }
		/// <summary>
		/// The recipients First Name
		/// </summary>
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		/// <summary>
		/// The recipients Lst Name
		/// </summary>
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		/// <summary>
		/// The receipients email address
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }
		/// <summary>
		/// The recipients address line 1 (primary address details)
		/// </summary>
		[JsonProperty("address_1")]
		public string AddressLine1 { get; set; }
		/// <summary>
		/// The recipients address line 2 (secondary address details - building name etc)
		/// </summary>
		[JsonProperty("address_2")]
		public string AddressLine2 { get; set; }
		/// <summary>
		/// The recipients city
		/// </summary>
		[JsonProperty("city")]
		public string City { get; set; }
		/// <summary>
		/// The recipients state
		/// </summary>
		[JsonProperty("state")]
		public string State { get; set; }
		/// <summary>
		/// The recipients postal code
		/// </summary>
		[JsonProperty("post_code")]
		public string PostCode { get; set; }
		/// <summary>
		/// The recipients country
		/// </summary>
		[JsonProperty("country")]
		public string Country { get; set; }
		/// <summary>
		/// The recipients phone number
		/// </summary>
		[JsonProperty("phone_number")]
		public string PhoneNumber { get; set; }
	}
}

