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
        /// Instantiates a new Credit Card from the json input
        /// </summary>
        /// <param name="json">Input from API calls</param>
        /// <returns>CreditCard</returns>
        public static CreditCard Parse(JsonValue json)
        {
            var obj = new CreditCard();
            if (json.ContainsKey("token"))
            {
                obj.ID = json["token"].ReadAs<string>();
                obj.Successful = true;
            }
            if (json.ContainsKey("card_holder"))
                obj.CardHolder = json["card_holder"].ReadAs<string>();
            if (json.ContainsKey("card_number"))
                obj.CardNumber = json["card_number"].ReadAs<string>();
            if (json.ContainsKey("card_expiry"))
                obj.ExpiryDate = json["card_expiry"].ReadAs<string>();

            return obj;
        }
    }
}
