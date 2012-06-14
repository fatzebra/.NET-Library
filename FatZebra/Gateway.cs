using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Json;
using System.Reflection;


namespace FatZebra
{
    public static class Gateway
    {
        private const string LIVE_GATEWAY_ADDRESS = "gateway.fatzebra.com.au";
        private const string SANDBOX_GATEWAY_ADDRESS = "gateway.sandbox.fatzebra.com.au";

        private static string _gatewayAddress = LIVE_GATEWAY_ADDRESS;
        private static string _version = "1.0";
        public static bool VerifySSL = true;

        /// <summary>
        /// Enables or Disabled Test Mode
        /// </summary>
        public static bool TestMode { get; set; }

        /// <summary>
        /// Enabled or Disabled Sandbox Mode
        /// </summary>
        public static bool SandboxMode { get; set; }

        /// <summary>
        /// The API Username
        /// </summary>
        public static string Username { get; set; }
        /// <summary>
        /// The API Token
        /// </summary>
        public static string Token { get; set; }
        /// <summary>
        /// The API Version
        /// </summary>
        public static string Version { get { return _version; } }

        /// <summary>
        /// The API Address
        /// </summary>
        public static string GatewayAddress
        {
            get
            {
                if (Gateway.SandboxMode)
                {
                    return SANDBOX_GATEWAY_ADDRESS;
                }
                else
                {
                    return LIVE_GATEWAY_ADDRESS;
                }
            }
        }

        /// <summary>
        /// The API Protocol - HTTP or HTTPS
        /// </summary>
        public static string Protocol
        {
            get
            {
                if (Gateway.VerifySSL)
                {
                    return "https";
                }
                else
                {
                    return "http";
                }
            }

            set
            {
                throw new Exception("Please set the protocol via VerifySSL");
            }
        }

        /// <summary>
        /// POST data to the Gateway
        /// </summary>
        /// <param name="uri">The URI</param>
        /// <param name="payload">JSON Payload to be posted</param>
        /// <returns>JSON Value response</returns>
        public static JsonValue Post(string uri, JsonObject payload)
        {
            var client = GetClient(uri);
            client.Method = "POST";

            var reqStream = client.GetRequestStream();
            StreamWriter sw = new StreamWriter(reqStream);

            sw.Write(payload.ToString());
            sw.Close();

            try
            {
                var result = (HttpWebResponse)client.GetResponse();
                var sr = new StreamReader(result.GetResponseStream());
                var jsonResponse = sr.ReadToEnd();
                sr.Close();

                return JsonValue.Parse(jsonResponse);
            }
            catch (WebException ex)
            {
                return Gateway.HandleException(ex);
            }
        }


        /// <summary>
        /// Perform a GET request against the Gateway
        /// </summary>
        /// <param name="uri">The URI to request</param>
        /// <returns>JSON Response</returns>
        public static JsonValue Get(string uri)
        {
            var client = GetClient(uri);
            try
            {
                var result = (HttpWebResponse)client.GetResponse();
                var sr = new StreamReader(result.GetResponseStream());
                var jsonResponse = sr.ReadToEnd();
                sr.Close();

                return JsonValue.Parse(jsonResponse);
            }
            catch (WebException e)
            {
                return Gateway.HandleException(e);
            }
        }

        /// <summary>
        /// PUT data to the Gateway
        /// </summary>
        /// <param name="uri">The URI for the request</param>
        /// <param name="payload">The JSON payload</param>
        /// <returns>JSON response</returns>
        public static JsonValue Put(string uri, JsonObject payload)
        {
            var client = GetClient(uri);
            client.Method = "PUT";

            var reqStream = client.GetRequestStream();
            StreamWriter sw = new StreamWriter(reqStream);

            sw.Write(payload.ToString());
            sw.Close();

            try
            {
                var result = (HttpWebResponse)client.GetResponse();
                var sr = new StreamReader(result.GetResponseStream());
                var jsonResponse = sr.ReadToEnd();
                sr.Close();

                return JsonValue.Parse(jsonResponse);
            }
            catch (WebException ex)
            {
                return Gateway.HandleException(ex);
            }
        }

        /// <summary>
        /// Ping the Fat Zebra Gateway to ensure its 'awake'
        /// </summary>
        /// <returns>true or false - if an exception occurrs we just let it through</returns>
        public static bool Ping()
        {
            bool success = false;

            var response = (HttpWebResponse)Gateway.GetClient("ping.json").GetResponse();
            success = response.StatusCode == HttpStatusCode.OK;

            return success;
        }


        #region Obsoleted Methods

        [Obsolete("This method has been replaced with Purchase.Create(...) and will be removed in future releases.")]
        public static Response Purchase(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip)
        {
            return FatZebra.Purchase.Create(amount, card_holder, card_number, card_expiry, cvv, reference, customer_ip);
        }

        [Obsolete("This method has been replaced with Purchase.Create(...) and will be removed in future releases.")]
        public static Response Purchase(int amount, string token, string cvv, string reference, string customer_ip)
        {
            return FatZebra.Purchase.Create(amount, token, cvv, reference, customer_ip);
        }

        [Obsolete("This method has been replaced with CreditCard.Create(...) and will be removed in future releases.")]
        public static Response TokenizeCard(string card_holder, string number, DateTime expiry, string cvv)
        {
            return CreditCard.Create(card_holder, number, expiry, cvv);
        }

        [Obsolete("This method has been replaced with Refund.Create(...) and will be removed in future releases.")]
        public static Response Refund(int amount, string originalTransactionNumber, string reference)
        {
            return FatZebra.Refund.Create(amount, originalTransactionNumber, reference);
        }

        [Obsolete("This method has been replaced with Plan.Create(...) and will be removed in future releases.")]
        public static Response CreatePlan(string name, string reference, string description, int amount)
        {
            return Plan.Create(name, reference, description, amount);
        }

        [Obsolete("This method has been replaced with Customer.Create(...) and will be removed in future releases.")]
        public static Response CreateCustomer(string first_name, string last_name, string reference, string email, string card_holder, string card_number, string cvv, DateTime expiry_date)
        {
            return Customer.Create(first_name, last_name, reference, email, card_holder, card_number, cvv, expiry_date);
        }

        [Obsolete("This method has been replaced with Subscription.Create(...) and will be removed in future releases.")]
        public static Response CreateSubscription(string customer_id, string plan_id, string frequency, string reference, DateTime start_date, bool is_active)
        {

            return Subscription.Create(customer_id, plan_id, frequency, reference, start_date, is_active);
        }

#endregion

        #region Private methods
        /// <summary>
        /// Formats the URI for the request
        /// </summary>
        /// <param name="endpoint">The endpoint of the request.</param>
        /// <returns>URI string</returns>
        private static string GetURI(string endpoint)
        {
            return String.Format("{0}://{1}/v{2}/{3}", Gateway.Protocol, Gateway.GatewayAddress, Gateway.Version, endpoint);
        }

        /// <summary>
        /// Creates a new web request client.
        /// </summary>
        /// <param name="endpoint">The end point of the request.</param>
        /// <returns>HttpWebRequest</returns>
        private static HttpWebRequest GetClient(string endpoint)
        {
            var client = (HttpWebRequest)System.Net.WebRequest.Create(Gateway.GetURI(endpoint));
            client.Credentials = new System.Net.NetworkCredential(Gateway.Username, Gateway.Token);
            client.UserAgent = String.Format("Official .NET {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            client.PreAuthenticate = true;

            return client;
        }

        /// <summary>
        /// Handles a WebException - if the exception is a 401 it raises an Auth exception, otherwise it tries to parse the error response.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>JsonValue or raises exception</returns>
        private static JsonValue HandleException(WebException ex)
        {
            var response = (HttpWebResponse)ex.Response;
            HttpStatusCode code = response.StatusCode;

            // If error is 401 then its exception
            // If its 404 or otherwise we can continue to return the response
            // Though if its 404 with text/html don't try it ;)

            if (code == HttpStatusCode.Unauthorized)
            {
                throw new Exception("HTTP 401 Unauthorized - Please check your username and token are correct");
            }

            if (response.ContentType.StartsWith("text/html") && code == HttpStatusCode.NotFound)
            {
                throw new Exception("HTTP 404 Not Found - Please check the URL or contact Fat Zebra support (support@fatzebra.com.au)");
            }

            var sr = new StreamReader(response.GetResponseStream());
            var jsonResponse = sr.ReadToEnd();
            sr.Close();

            return JsonValue.Parse(jsonResponse);
        }
        #endregion
    }
}
