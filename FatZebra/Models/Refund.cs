using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public class Refund : IRecord
    {
        /// <summary>
        /// The Transaction ID
        /// </summary>
		[JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Indicates the success of the refund
        /// </summary>
		[JsonProperty("successful")]
		public bool Successful { get; set; }

        /// <summary>
        /// The Refund amount
        /// </summary>
		[JsonProperty("amount")]
		public int Amount { get; set; }

        /// <summary>
        /// The authorization ID
        /// </summary>
		[JsonProperty("authorization")]
		public string Authorization { get; set; }

        /// <summary>
        /// Result Message
        /// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }

        /// <summary>
		/// The response code
        /// </summary>
		[JsonProperty("response_code")]
		public string ResponseCode { get; set; }

        /// <summary>
        /// Performs a refund of an existing transaction.
        /// </summary>
        /// <param name="amount">The amount to be refunded as an integer.</param>
        /// <param name="originalTransactionNumber">The original transaction to apply the refund against.</param>
        /// <param name="reference">The reference for the refund.</param>
        /// <returns>Response</returns>
		public static Response<Refund> Create(int amount, string originalTransactionNumber, string reference)
        {
			var req = new Requests.Refund {
				TransactionID = originalTransactionNumber,
				Amount = amount,
				Reference = reference,
				TestMode = Gateway.TestMode
			};

			return Gateway.Post<Refund>("refunds.json", req);
        }
    }
}
