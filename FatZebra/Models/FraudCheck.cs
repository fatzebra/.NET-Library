using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FatZebra
{
	public class FraudCheck
	{
		/// <summary>
		/// Order items associated with the transaction
		/// </summary>
		[JsonProperty("items")]
		public List<OrderItem> OrderItems { get; set; }

		/// <summary>
		/// The order shipping address
		/// </summary>
		[JsonProperty("shipping_address")]
		public ShippingAddress ShippingAddress { get; set; }

		/// <summary>
		/// The order customer details
		/// </summary>
		[JsonProperty("customer")]
		public OrderCustomer Customer { get; set; }


		[JsonProperty("recipients")]
		public List<Recipient> Recipients { get; set; }

		/// <summary>
		/// The computed DeviceID from the customers browser.
		/// </summary>
		[JsonProperty("device_id")]
		public string DeviceID { get; set; }

		public FraudCheck()
		{
			this.OrderItems = new List<OrderItem> ();
			this.Recipients = new List<Recipient> ();
			this.ShippingAddress = new ShippingAddress ();
			this.Customer = new Customer ();
		}
	}
}

