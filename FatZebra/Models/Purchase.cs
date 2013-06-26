using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Purchase : IRecord
    {
        /// <summary>
        /// The purchase transaction ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Indicates whether the transaction was successful or not
        /// </summary>
        public bool Successful { get; set; }
        /// <summary>
        /// The authorization ID
        /// </summary>
        public string Authorization { get; set; }
        /// <summary>
        /// The card number for the transaction (masked)
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// The card holder name
        /// </summary>
        public string CardHolder { get; set; }
        /// <summary>
        /// The card expiry date
        /// </summary>
        public string CardExpiry { get; set; }
        /// <summary>
        /// The card token
        /// </summary>
        public string CardToken { get; set; }

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
        /// The purchase amount as an integer
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// The purchase amount as a decimal
        /// </summary>
        public Double DecimalAmount { get; set; }
        /// <summary>
        /// The purchase response message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The purchase reference (for example, your invoice number)
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Instantiates a new Credit Card from the json input
        /// </summary>
        /// <param name="json">Input from API calls</param>
        /// <returns>Purchase</returns>
        internal static Purchase Parse(System.Json.JsonValue json)
        {
            var obj = new Purchase();

            if (json.ContainsKey("id") && json["id"] != null)
                obj.ID = json["id"].ReadAs<string>();
            if (json.ContainsKey("authorization") && json["authorization"] != null)
                obj.Authorization = json["authorization"].ReadAs<string>();
            if (json.ContainsKey("successful") && json["successful"] != null)
                obj.Successful = json["successful"].ReadAs<bool>();
            if (json.ContainsKey("card_number") && json["card_number"] != null)
                obj.CardNumber = json["card_number"].ReadAs<string>();
            if (json.ContainsKey("card_holder") && json["card_holder"] != null)
                obj.CardHolder = json["card_holder"].ReadAs<string>();
            if (json.ContainsKey("card_expiry") && json["card_expiry"] != null)
                obj.CardExpiry = json["card_expiry"].ReadAs<string>();
            if (json.ContainsKey("card_token") && json["card_token"] != null)
                obj.CardToken = json["card_token"].ReadAs<string>();
            if (json.ContainsKey("amount") && json["amount"] != null)
                obj.Amount = json["amount"].ReadAs<int>();
            if (json.ContainsKey("decimal_amount") && json["decimal_amount"] != null)
                obj.DecimalAmount = json["decimal_amount"].ReadAs<Double>();
            if (json.ContainsKey("message") && json["message"] != null)
                obj.Message = json["message"].ReadAs<string>();
            if (json.ContainsKey("reference") && json["reference"] != null)
                obj.Reference = json["reference"].ReadAs<string>();

            return obj;
        }

		/// <summary>
		/// Purchase with card data
		/// </summary>
		/// <param name="amount">purchase amount as an integer</param>
		/// <param name="card_holder">card holders name</param>
		/// <param name="card_number">card number</param>
		/// <param name="card_expiry">card expiry</param>
		/// <param name="cvv">CVV number</param>
		/// <param name="reference">purchase reference (invoice number or similar)</param>
		/// <param name="customer_ip">customers IP address</param>
		/// <returns>Response</returns>
		public static Response Create (int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip)
		{
			return Purchase.Create(amount, card_holder, card_number, card_expiry, cvv, reference, customer_ip, "AUD");
		}

        /// <summary>
        /// Purchase with card data, specifying the currency code
        /// </summary>
        /// <param name="amount">purchase amount as an integer</param>
        /// <param name="card_holder">card holders name</param>
        /// <param name="card_number">card number</param>
        /// <param name="card_expiry">card expiry</param>
        /// <param name="cvv">CVV number</param>
        /// <param name="reference">purchase reference (invoice number or similar)</param>
        /// <param name="customer_ip">customers IP address</param>
		/// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes)</para>
        /// <returns>Response</returns>
        public static Response Create(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip, string currency)
        {
            var payload = new JsonObject();
            payload.Add("amount", amount);
            payload.Add("reference", reference);
            payload.Add("customer_ip", customer_ip);

            payload.Add("card_number", card_number);
            payload.Add("card_holder", card_holder);
            payload.Add("card_expiry", card_expiry.ToString("MM/yyyy"));
            payload.Add("cvv", cvv);
			payload.Add ("currency", currency);
            payload.Add("test", Gateway.TestMode);

            return Response.ParsePurchase(Gateway.Post("purchases.json", payload));
        }

		/// <summary>
		/// Purchase with a tokenized card
		/// </summary>
		/// <param name="amount">purchase amount as integer</param>
		/// <param name="token">card token</param>
		/// <param name="cvv">card CVV</param>
		/// <param name="reference">purchase reference (e.g. invoice number)</param>
		/// <param name="customer_ip">the custokers IP address</param>
		/// <returns>Response</returns>
		public static Response Create(int amount, string token, string cvv, string reference, string customer_ip)
		{
			return Purchase.Create(amount, token, cvv, reference, customer_ip, "AUD");
		}

        /// <summary>
        /// Purchase with a tokenized card, specifying the currency code
        /// </summary>
        /// <param name="amount">purchase amount as integer</param>
        /// <param name="token">card token</param>
        /// <param name="cvv">card CVV</param>
        /// <param name="reference">purchase reference (e.g. invoice number)</param>
        /// <param name="customer_ip">the custokers IP address</param>
		/// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes)</para>
        /// <returns>Response</returns>
        public static Response Create(int amount, string token, string cvv, string reference, string customer_ip, string currency)
        {
            var payload = new JsonObject();
            payload.Add("amount", amount);
            payload.Add("reference", reference);
            payload.Add("customer_ip", customer_ip);

            payload.Add("card_token", token);
            payload.Add("cvv", cvv);
			payload.Add("currency", currency);
            payload.Add("test", Gateway.TestMode);

            return Response.ParsePurchase(Gateway.Post("purchases.json", payload));
        }

        /// <summary>
        /// Refunds the current transaction
        /// </summary>
        /// <param name="amount">The amount to refund</param>
        /// <param name="reference">The reference for the refund</param>
        /// <returns>Response</returns>
        public Response Refund(int amount, string reference)
        {
            return FatZebra.Refund.Create(amount, this.ID, reference);
        }
    }
}
