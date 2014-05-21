using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public class CreditCard : IRecord
    {
        /// <summary>
        /// Indicated the card was tokenized successfully
        /// </summary>
		public bool Successful { get { return this.ID != null; } }
        /// <summary>
        /// The card token
        /// </summary>
		[JsonProperty("token")]
		public string ID { get; set; }
        /// <summary>
        /// The card holders name
        /// </summary>
		[JsonProperty("card_holder")]
		public string CardHolder { get; set; }
        /// <summary>
        /// The card number (masked)
        /// </summary>
		[JsonProperty("card_number")]
		public string CardNumber { get; set; }
        /// <summary>
        /// The card expiry date
        /// </summary>
		[JsonProperty("expiry_date")]
		public string ExpiryDate { get; set; }

        /// <summary>
        /// The card type
        /// </summary>
        public string CardType
        {
            get
            {
                if (this.CardNumber == null) return "Unknown";

                switch (this.CardNumber.ToCharArray()[0])
                {
                    case '4':
                        return "VISA";
                    case '5':
                        return "MasterCard";
                    default:
                        return "Unload";
                }
            }
        }

        /// <summary>
        /// Tokenizes a Credit Card Number to be used with Purchase(amount, token, cvv, reference, customer_ip)
        /// </summary>
        /// <param name="card_holder">The card holders name</param>
        /// <param name="number">The card number</param>
        /// <param name="expiry">The card expiry date</param>
        /// <param name="cvv">The card CVV</param>
        /// <returns></returns>
		public static Response<CreditCard> Create(string card_holder, string number, DateTime expiry, string cvv)
        {
			var req = new Requests.CreditCard {
				CardHolder = card_holder,
				CardNumber = number,
				ExpiryDate = expiry,
				SecurityCode = cvv,
				TestMode = Gateway.TestMode
			};

			return Gateway.Post<CreditCard>("credit_cards.json", req);
        }

		/// <summary>
		/// Fetches a Tokenized Credit Card from the database
		/// If the card cannot be found Null is return.
		/// </summary>
		/// <param name='identifier'>
		/// The card token
		/// </param>
		public static CreditCard Find (string identifier)
		{
			var response = Gateway.Get<CreditCard>(String.Format("credit_cards/{0}.json", identifier));
			var card = response.Result;

			if (response.Successful && card.CardType != "Unknown") {
				return card;
			} else {
				return null;
			}
		}
    }
}
