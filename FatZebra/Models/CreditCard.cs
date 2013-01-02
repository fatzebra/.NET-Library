using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class CreditCard : IRecord
    {
        /// <summary>
        /// Indicated the card was tokenized successfully
        /// </summary>
        public bool Successful { get; set; }
        /// <summary>
        /// The card token
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// The card holders name
        /// </summary>
        public string CardHolder { get; set; }
        /// <summary>
        /// The card number (masked)
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// The card expiry date
        /// </summary>
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
        /// Instantiates a new Credit Card from the json input
        /// </summary>
        /// <param name="json">Input from API calls</param>
        /// <returns>CreditCard</returns>
        public static CreditCard Parse (JsonValue json)
		{
			if (json == null) {
				return new CreditCard ();
			}

            var obj = new CreditCard();
            if (json.ContainsKey("token") && json["token"] != null)
            {
                obj.ID = json["token"].ReadAs<string>();
                obj.Successful = true;
            }
            if (json.ContainsKey("card_holder") && json["card_holder"] != null)
                obj.CardHolder = json["card_holder"].ReadAs<string>();
            if (json.ContainsKey("card_number") && json["card_number"] != null)
                obj.CardNumber = json["card_number"].ReadAs<string>();
            if (json.ContainsKey("card_expiry") && json["card_expiry"] != null)
                obj.ExpiryDate = json["card_expiry"].ReadAs<string>();

            return obj;
        }

        /// <summary>
        /// Tokenizes a Credit Card Number to be used with Purchase(amount, token, cvv, reference, customer_ip)
        /// </summary>
        /// <param name="card_holder">The card holders name</param>
        /// <param name="number">The card number</param>
        /// <param name="expiry">The card expiry date</param>
        /// <param name="cvv">The card CVV</param>
        /// <returns></returns>
        public static Response Create(string card_holder, string number, DateTime expiry, string cvv)
        {
            var payload = new JsonObject();
            payload.Add("card_holder", card_holder);
            payload.Add("card_number", number);
            payload.Add("card_expiry", expiry.ToString("MM/yyyy"));
            payload.Add("cvv", cvv);
            payload.Add("test", Gateway.TestMode);

            return Response.ParseTokenized(Gateway.Post("credit_cards.json", payload));
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
			var response = Response.ParseTokenized (Gateway.Get(String.Format("credit_cards/{0}.json", identifier)));
			var card = (CreditCard)response.Result;

			if (response.Successful && card.CardType != "Unknown") {
				return card;
			} else {
				return null;
			}
		}
    }
}
