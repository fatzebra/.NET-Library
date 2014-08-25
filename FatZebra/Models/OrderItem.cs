using System;
using Newtonsoft.Json;

namespace FatZebra
{
	public class OrderItem
	{
		/// <summary>
		/// The items product code
		/// </summary>
		[JsonProperty("product_code")]
		public string ProductCode { get; set; }
		/// <summary>
		/// The Items SKU number (e.g. base product code)
		/// </summary>
		[JsonProperty("sku")]
		public string SKU { get; set; }
		/// <summary>
		/// The quantity of items on the order
		/// </summary>
		[JsonProperty("qty")]
		public int Quantity { get; set; }
		/// <summary>
		/// The items description. Limited to 26 characters. If longer truncate and add details to GiftMessage.
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }
		/// <summary>
		/// The base item cost for the minimum quantity
		/// </summary>
		/// <value>The item cost.</value>
		[JsonProperty("cost")]
		public float ItemCost { get; set; }
		/// <summary>
		/// The line total (ItemCost * Quantity)
		/// </summary>
		[JsonProperty("line_total")]
		public float LineTotal { get; set; }
		/// <summary>
		/// The shipping tracking number
		/// </summary>
		[JsonProperty("tracking_number")]
		public string TrackingNumber { get; set; }
		/// <summary>
		/// Gift message entered by the customer, or any additional description data.
		/// </summary>
		[JsonProperty("gift_message")]
		public string GiftMessage { get; set; }
		/// <summary>
		/// The manufactures part number
		/// </summary>
		[JsonProperty("part_number")]
		public string PartNumber { get; set; }
		/// <summary>
		/// Any comments entered by the customer for the shipping.
		/// </summary>
		[JsonProperty("shipping_comments")]
		public string ShippingComments { get; set; }
	}
}

