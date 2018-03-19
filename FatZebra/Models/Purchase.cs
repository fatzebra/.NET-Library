using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace FatZebra
{
    public class Purchase : IRecord
    {
        /// <summary>
        /// The purchase transaction ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// Indicates whether the transaction was successful or not
        /// </summary>
        [JsonProperty("successful")]
        public bool Successful { get; set; }
        /// <summary>
        /// The authorization ID
        /// </summary>
        [JsonProperty("authorization")]
        public string Authorization { get; set; }
        /// <summary>
        /// The card number for the transaction (masked)
        /// </summary>
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }
        /// <summary>
        /// The card holder name
        /// </summary>
        [JsonProperty("card_holder")]
        public string CardHolder { get; set; }
        /// <summary>
        /// The card expiry date
        /// </summary>
        [JsonProperty("card_expiry")]
        public string CardExpiry { get; set; }
        /// <summary>
        /// The card token
        /// </summary>
        [JsonProperty("card_token")]
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
        [JsonProperty("amount")]
        public int Amount { get; set; }
        /// <summary>
        /// The purchase amount as a decimal
        /// </summary>
        [JsonProperty("decimal_amount")]
        public Double DecimalAmount { get; set; }
        /// <summary>
        /// The purchase response message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// The purchase reference (for example, your invoice number)
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// The acquirer response code (00, 05 etc)
        /// </summary>
        /// <value>The response code.</value>
        [JsonProperty("response_code")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// The fraud check result (if enabled for this transaction)
        /// </summary>
        /// <value>The fraud check result.</value>
        [JsonProperty("fraud_result")]
        [DefaultValue(FraudResult.Unknown)]
        public FraudResult FraudCheckResult { get; set; }

        /// <summary>
        /// Any fraud-check response messages (this may be populated if there are any fraud check errors, or fraud rules are triggered).
        /// </summary>
        /// <value>The fraud messages.</value>
        [DefaultValue(default(List<string>))]
        [JsonProperty("fraud_messages")]
        public IList<string> FraudMessages { get; set; } 

        /// <summary>
        /// Fetches a purchase from the server.
        /// </summary>
        /// <param name="reference">The purchase reference or Fat Zebra ID.</param>
        public static Response<Purchase> Get(string reference)
        {
            return Gateway.Get<Purchase> (String.Format ("purchases/#{0}.json", reference));
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
        public static Response<Purchase> Create (int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip)
        {
            return Purchase.Create(amount, card_holder, card_number, card_expiry, cvv, reference, customer_ip, "AUD", null);
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
        public static Response<Purchase> Create(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip, string currency)
        {
            return Purchase.Create (amount, card_holder, card_number, card_expiry, cvv, reference, customer_ip, currency, null);
        }

        /// <summary>
        /// Purchase with card data, specifying the currency code, and running fraud checks
        /// </summary>
        /// <param name="amount">purchase amount as an integer</param>
        /// <param name="card_holder">card holders name</param>
        /// <param name="card_number">card number</param>
        /// <param name="card_expiry">card expiry</param>
        /// <param name="cvv">CVV number</param>
        /// <param name="reference">purchase reference (invoice number or similar)</param>
        /// <param name="customer_ip">customers IP address</param>
        /// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes)</param>
        /// <param name="fraud_details">The fraud check details</param>
        /// <returns>Response</returns>
        public static Response<Purchase> Create(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip, string currency, FraudCheck fraud_details)
        {
            return Purchase.Create(amount, card_holder, card_number, card_expiry, cvv, reference, customer_ip, currency, fraud_details, null);
        }

        /// <summary>
        /// Purchase with card data, specifying the currency code, and running fraud checks (optional). This method also accepts extra flags
        /// </summary>
        /// <param name="amount">purchase amount as an integer</param>
        /// <param name="card_holder">card holders name</param>
        /// <param name="card_number">card number</param>
        /// <param name="card_expiry">card expiry</param>
        /// <param name="cvv">CVV number</param>
        /// <param name="reference">purchase reference (invoice number or similar)</param>
        /// <param name="customer_ip">customers IP address</param>
        /// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes)</param>
        /// <param name="fraud_details">The fraud check details - optional - pass null if not used</param>
        /// <param name="extra">Extra params for the request (such as ECM flags) - optional - pass null if not used</param>
        /// <returns>Response</returns>
        public static Response<Purchase> Create(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip, string currency, FraudCheck fraud_details, Dictionary<String, Object> extra)
        {
            var req = new Requests.Purchase {
                Amount = amount,
                Reference = reference,
                CustomerIP = customer_ip,
                CardNumber = card_number,
                CardHolder = card_holder,
                CardExpiry = card_expiry,
                SecurityCode = cvv,
                Currency = currency,
                TestMode = Gateway.TestMode,
                FraudDetails = fraud_details,
                Extra = extra
            };

            return Gateway.Post<Purchase>("purchases.json", req);
        }

        /// <summary>
        /// Purchase with a tokenized card, specifying the currency code and running fraud checks
        /// </summary>
        /// <param name="amount">purchase amount as integer</param>
        /// <param name="token">card token</param>
        /// <param name="cvv">card CVV</param>
        /// <param name="reference">purchase reference (e.g. invoice number)</param>
        /// <param name="customer_ip">the custokers IP address</param>
        /// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes for full list)</param>
        /// <param name="fraud_details">The fraud check details</param>
        /// <param name="extra">Extra params for the request (such as ECM flags) - optional - pass null if not used</param>
        /// <returns>Response</returns>
        public static Response<Purchase> Create(int amount, string token, string cvv, string reference, string customer_ip, string currency, FraudCheck fraud_details, Dictionary<String, Object> extra)
        {
            var req = new Requests.Purchase {
                Amount = amount,
                Reference = reference,
                CustomerIP = customer_ip,
                CardToken = token,
                SecurityCode = cvv,
                Currency = currency,
                TestMode = Gateway.TestMode,
                FraudDetails = fraud_details,
                Extra = extra
            };

            return Gateway.Post<Purchase>("purchases.json", req);
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
        public static Response<Purchase> Create(int amount, string token, string cvv, string reference, string customer_ip)
        {
            return Purchase.Create(amount, token, cvv, reference, customer_ip, "AUD", null);
        }

        /// <summary>
        /// Purchase with a tokenized card, specifying the currency code
        /// </summary>
        /// <param name="amount">purchase amount as integer</param>
        /// <param name="token">card token</param>
        /// <param name="cvv">card CVV</param>
        /// <param name="reference">purchase reference (e.g. invoice number)</param>
        /// <param name="customer_ip">the custokers IP address</param>
        /// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes for full list)</para>
        /// <returns>Response</returns>
        public static Response<Purchase> Create(int amount, string token, string cvv, string reference, string customer_ip, string currency)
        {
            return Purchase.Create(amount, token, cvv, reference, customer_ip, currency, null);
        }

        /// <summary>
        /// Purchase with a tokenized card, specifying the currency code and running fraud checks
        /// </summary>
        /// <param name="amount">purchase amount as integer</param>
        /// <param name="token">card token</param>
        /// <param name="cvv">card CVV</param>
        /// <param name="reference">purchase reference (e.g. invoice number)</param>
        /// <param name="customer_ip">the custokers IP address</param>
        /// <param name="currency">The three-letter ISO-4217 currency code (see http://en.wikipedia.org/wiki/ISO_4217#Active_codes for full list)</para>
        /// <param name="fraud_details">The fraud check details</param>
        /// <returns>Response</returns>
        public static Response<Purchase> Create(int amount, string token, string cvv, string reference, string customer_ip, string currency, FraudCheck fraud_details)
        {
            return Purchase.Create(amount, token, cvv, reference, customer_ip, currency, fraud_details, null);
        }

        /// <summary>
        /// Refunds the current transaction
        /// </summary>
        /// <param name="amount">The amount to refund</param>
        /// <param name="reference">The reference for the refund</param>
        /// <returns>Response</returns>
        public Response<Refund> Refund(int amount, string reference)
        {
            return FatZebra.Refund.Create(amount, this.ID, reference);
        }
    }
}
