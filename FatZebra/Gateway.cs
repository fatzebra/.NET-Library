using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace FatZebra
{
    public static class Gateway
    {
        private const string LIVE_GATEWAY_ADDRESS = "gateway.fatzebra.com.au";
        private const string SANDBOX_GATEWAY_ADDRESS = "gateway.sandbox.fatzebra.com.au";
		private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        private static string _version = "1.0";
		private static string _gwAddress = Gateway.LIVE_GATEWAY_ADDRESS;
		private static bool _sandbox = false;
        public static bool VerifySSL = true;
		public static bool VersionPrefix = true;

        /// <summary>
        /// Enables or Disabled Test Mode
        /// </summary>
        public static bool TestMode { get; set; }

        /// <summary>
        /// Enabled or Disabled Sandbox Mode
        /// </summary>
        public static bool SandboxMode { 
			get {
				return _sandbox;
			}

			set {
				_sandbox = value;
				if (value) {
					_gwAddress = Gateway.SANDBOX_GATEWAY_ADDRESS;
				} else {
					_gwAddress = Gateway.LIVE_GATEWAY_ADDRESS;
				}
			}
		
		}

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
        public static string GatewayAddress {
			get {
				return _gwAddress;
			}

			set {
				_gwAddress = value;
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

		public static IWebProxy Proxy 
		{
			get; set;
		}

		/// <summary>
		/// POST data to the Gateway
		/// </summary>
		/// <param name="uri">The URI</param>
		/// <param name="payload">JSON Payload (IRequest) to be posted</param>
		/// <returns>JSON Value response</returns>
		public static Response<T> Post<T>(string uri, IRequest payload)
		{
			var client = GetClient(uri);
			client.Method = "POST";

			try
			{
				var reqStream = client.GetRequestStream();
				StreamWriter sw = new StreamWriter(reqStream);
				sw.Write(JsonConvert.SerializeObject (payload, Formatting.Indented, _serializerSettings));
				sw.Close();

				var result = (HttpWebResponse)client.GetResponse();
				var sr = new StreamReader(result.GetResponseStream());
				Response<T> response = JsonConvert.DeserializeObject<Response<T>>(sr.ReadToEnd());
				sr.Close();
				return response;
			}
			catch (WebException ex)
			{
				return Gateway.HandleException<T>(ex);
			}
		}


        /// <summary>
        /// Perform a GET request against the Gateway
        /// </summary>
        /// <param name="uri">The URI to request</param>
        /// <returns>JSON Response</returns>
		public static Response<T> Get<T>(string uri) 
		{
			var client = GetClient (uri);
			try 
			{
				var result = (HttpWebResponse)client.GetResponse();
				var sr = new StreamReader(result.GetResponseStream());
				Response<T> response = JsonConvert.DeserializeObject<Response<T>>(sr.ReadToEnd());
				sr.Close();

				return response;
			}
			catch(WebException e) 
			{
				return Gateway.HandleException<T>(e);
			}
		}

		/// <summary>
		/// PUT data to the Gateway
		/// </summary>
		/// <param name="uri">The URI for the request</param>
		/// <param name="payload">The JSON payload (IRequest)</param>
		/// <returns>JSON response</returns>
		public static Response<T> Put<T>(string uri, IRequest payload)
		{
			var client = GetClient(uri);
			client.Method = "PUT";

			var reqStream = client.GetRequestStream();
			StreamWriter sw = new StreamWriter(reqStream);

			sw.Write(JsonConvert.SerializeObject (payload, Formatting.Indented, _serializerSettings));
			sw.Close();

			try
			{
				var result = (HttpWebResponse)client.GetResponse();
				var sr = new StreamReader(result.GetResponseStream());
				Response<T> response = JsonConvert.DeserializeObject<Response<T>>(sr.ReadToEnd());
				sr.Close();

				return response;
			}
			catch (WebException ex)
			{
				return Gateway.HandleException<T>(ex);
			}
		}

		/// <summary>
		/// Sends a DELETE request to the gateway
		/// </summary>
		/// <param name="uri">The URI for the request</param>
		/// <returns>Indication of success</returns>
		public static bool Delete (string uri)
		{
			bool success = false;
			var client = GetClient (uri);
			client.Method = "DELETE";

			try {
				var response = (HttpWebResponse)client.GetResponse ();
				success = response.StatusCode == HttpStatusCode.OK;
				return success;
			} catch (WebException) {
				return false;
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
		public static Response<Purchase> Purchase(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip)
        {
            return FatZebra.Purchase.Create(amount, card_holder, card_number, card_expiry, cvv, reference, customer_ip);
        }

        [Obsolete("This method has been replaced with Purchase.Create(...) and will be removed in future releases.")]
		public static Response<Purchase> Purchase(int amount, string token, string cvv, string reference, string customer_ip)
        {
            return FatZebra.Purchase.Create(amount, token, cvv, reference, customer_ip);
        }

        [Obsolete("This method has been replaced with CreditCard.Create(...) and will be removed in future releases.")]
		public static Response<CreditCard> TokenizeCard(string card_holder, string number, DateTime expiry, string cvv)
        {
            return CreditCard.Create(card_holder, number, expiry, cvv);
        }

        [Obsolete("This method has been replaced with Refund.Create(...) and will be removed in future releases.")]
		public static Response<Refund> Refund(int amount, string originalTransactionNumber, string reference)
        {
            return FatZebra.Refund.Create(amount, originalTransactionNumber, reference);
        }

        [Obsolete("This method has been replaced with Plan.Create(...) and will be removed in future releases.")]
		public static Response<Plan> CreatePlan(string name, string reference, string description, int amount)
        {
            return Plan.Create(name, reference, description, amount);
        }

        [Obsolete("This method has been replaced with Customer.Create(...) and will be removed in future releases.")]
		public static Response<Customer> CreateCustomer(string first_name, string last_name, string reference, string email, string card_holder, string card_number, string cvv, DateTime expiry_date)
        {
            return Customer.Create(first_name, last_name, reference, email, card_holder, card_number, cvv, expiry_date);
        }

        [Obsolete("This method has been replaced with Subscription.Create(...) and will be removed in future releases.")]
		public static Response<Subscription> CreateSubscription(string customer_id, string plan_id, string frequency, string reference, DateTime start_date, bool is_active)
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
        private static string GetURI (string endpoint)
		{
			if (VersionPrefix) {
				return String.Format ("{0}://{1}/v{2}/{3}", Gateway.Protocol, Gateway.GatewayAddress, Gateway.Version, endpoint);
			} else {
				return String.Format ("{0}://{1}/{2}", Gateway.Protocol, Gateway.GatewayAddress, endpoint);
			}
        }

        /// <summary>
        /// Creates a new web request client.
        /// </summary>
        /// <param name="endpoint">The end point of the request.</param>
        /// <returns>HttpWebRequest</returns>
        private static HttpWebRequest GetClient (string endpoint)
		{
			var client = (HttpWebRequest)System.Net.WebRequest.Create (Gateway.GetURI (endpoint));
			client.Credentials = new System.Net.NetworkCredential (Gateway.Username, Gateway.Token);
			client.UserAgent = String.Format ("Official .NET {0}", Assembly.GetCallingAssembly().GetName().Version.ToString());
			client.PreAuthenticate = true;
			client.ContentType = "application/json";
			if (Gateway.Proxy != null) {
				client.Proxy = Gateway.Proxy;
			}

            return client;
        }

        /// <summary>
        /// Handles a WebException - if the exception is a 401 it raises an Auth exception, otherwise it tries to parse the error response.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>JsonValue or raises exception</returns>
		private static Response<T> HandleException<T> (WebException ex)
		{
			if (ex.Response == null) {
				throw new Exception(String.Format("Error connecting to Gateway: {0}", ex.Status), ex);
			}

			var response = (HttpWebResponse)ex.Response;
            
			if (response == null && ex.Status == WebExceptionStatus.ConnectFailure) {
				throw new Exception("Error connecting to gateway. Please verify you are able to communicate with the gateway address over HTTPS.", ex);
			}

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
			var responseObject = JsonConvert.DeserializeObject<Response<T>>(sr.ReadToEnd());
            sr.Close();

			return responseObject;
        }
        #endregion
    }
}
