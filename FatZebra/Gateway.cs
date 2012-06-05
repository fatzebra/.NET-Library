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
    public class Gateway
    {
        private const string LIVE_GATEWAY_ADDRESS = "gateway.fatzebra.com.au";
        private const string SANDBOX_GATEWAY_ADDRESS = "gateway.sandbox.fatzebra.com.au";

        private string _gatewayAddress = LIVE_GATEWAY_ADDRESS;
        private string _version = "1.0";
        public bool VerifySSL = true;

        /// <summary>
        /// Enables or Disabled Test Mode
        /// </summary>
        public bool TestMode { get; set; }

        /// <summary>
        /// Enabled or Disabled Sandbox Mode
        /// </summary>
        public bool SandboxMode { get; set; }

        /// <summary>
        /// The API Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The API Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The API Version
        /// </summary>
        public string Version { get { return _version; } }

        /// <summary>
        /// The API Address
        /// </summary>
        public string GatewayAddress
        {
            get
            {
                if (this.SandboxMode)
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
        public string Protocol
        {
            get
            {
                if (this.VerifySSL)
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
        /// Instantiates a new instance of the Gateway
        /// </summary>
        /// <param name="username">The API Username</param>
        /// <param name="token">The API Token</param>
        public Gateway(string username, string token)
        {
            this.Username = username;
            this.Token = token;
            this.SandboxMode = false;
            this.TestMode = false;
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
        public Response Purchase(int amount, string card_holder, string card_number, DateTime card_expiry, string cvv, string reference, string customer_ip)
        {
            var client = GetClient("purchases.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("amount", amount);
            payload.Add("reference", reference);
            payload.Add("customer_ip", customer_ip);
            
            payload.Add("card_number", card_number);
            payload.Add("card_holder", card_holder);
            payload.Add("card_expiry", card_expiry.ToString("MM/yyyy"));
            payload.Add("cvv", cvv);
            payload.Add("test", this.TestMode);

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

                return Response.ParsePurchase(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Purchase");
            }
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
        public Response Purchase(int amount, string token, string cvv, string reference, string customer_ip)
        {
            var client = GetClient("purchases.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("amount", amount);
            payload.Add("reference", reference);
            payload.Add("customer_ip", customer_ip);

            payload.Add("card_token", token);
            payload.Add("cvv", cvv);
            payload.Add("test", this.TestMode);

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

                return Response.ParsePurchase(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Purchase");
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
        public Response TokenizeCard(string card_holder, string number, DateTime expiry, string cvv)
        {
            var client = GetClient("credit_cards.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("card_holder", card_holder);
            payload.Add("card_number", number);
            payload.Add("card_expiry", expiry.ToString("MM/yyyy"));
            payload.Add("cvv", cvv);
            payload.Add("test", this.TestMode);

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

                return Response.ParseTokenized(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Tokenize");
            }
        }

        /// <summary>
        /// Performs a regund of an existing transaction.
        /// </summary>
        /// <param name="amount">The amount to be refunded as an integer.</param>
        /// <param name="originalTransactionNumber">The original transaction to apply the refund against.</param>
        /// <param name="reference">The reference for the refund.</param>
        /// <returns>Response</returns>
        public Response Refund(int amount, string originalTransactionNumber, string reference)
        {
            var client = GetClient("refunds.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("transaction_id", originalTransactionNumber);
            payload.Add("amount", amount);
            payload.Add("reference", reference);
            payload.Add("test", this.TestMode);

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

                return Response.ParseRefund(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Refund");
            }
        }

        /// <summary>
        /// Creates a new Plan
        /// </summary>
        /// <param name="name">The name of the plan</param>
        /// <param name="reference">The reference</param>
        /// <param name="description">The plan description</param>
        /// <param name="amount">The plan amount, as an integer</param>
        /// <returns>Response</returns>
        public Response CreatePlan(string name, string reference, string description, int amount)
        {
            var client = GetClient("plans.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("name", name);
            payload.Add("description", description);
            payload.Add("reference", reference);
            payload.Add("amount", amount);
            payload.Add("test", this.TestMode);

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

                return Response.ParsePlan(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Plan");
            }
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="first_name">The customers first name</param>
        /// <param name="last_name">The customers last name</param>
        /// <param name="reference">Your reference (e.g. customer ID)</param>
        /// <param name="email">The customers email address</param>
        /// <param name="card_holder">The card holders name</param>
        /// <param name="card_number">The customers credit card number</param>
        /// <param name="cvv">The CVV for the card</param>
        /// <param name="expiry_date">The expiry date for the card</param>
        /// <returns>Response</returns>
        public Response CreateCustomer(string first_name, string last_name, string reference, string email, string card_holder, string card_number, string cvv, DateTime expiry_date)
        {
            var client = GetClient("customers.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("first_name", first_name);
            payload.Add("last_name", last_name);
            payload.Add("reference", reference);
            payload.Add("email", email);
            var card = new JsonObject();
            card.Add("card_number", card_number);
            card.Add("card_holder", card_holder);
            card.Add("cvv", cvv);
            card.Add("expiry_date", expiry_date.ToString("MM/yyyy"));

            payload.Add("card", card);

            payload.Add("test", this.TestMode);

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

                return Response.ParseCustomer(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Customer");
            }
        }


        public Response CreateSubscription(string customer_id, string plan_id, string frequency, string reference, DateTime start_date, bool is_active)
        {
            var client = GetClient("subscriptions.json");
            client.Method = "POST";

            var payload = new JsonObject();
            payload.Add("customer", customer_id);
            payload.Add("plan", plan_id);
            payload.Add("reference", reference);
            payload.Add("is_active", is_active);
            payload.Add("start_date", start_date.ToString("yyyy-MM-dd"));
            payload.Add("frequency", frequency);
            payload.Add("test", this.TestMode);
            

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

                return Response.ParseSubscription(jsonResponse);
            }
            catch (WebException ex)
            {
                return this.HandleException(ex, "Subscription");
            }
        }


        /// <summary>
        /// Ping the Fat Zebra gateway to ensure its 'awake'
        /// </summary>
        /// <returns>true or false - if an exception occurrs we just let it through</returns>
        public bool Ping()
        {
            bool success = false;

            var response = (HttpWebResponse)this.GetClient("ping.json").GetResponse();
            success = response.StatusCode == HttpStatusCode.OK;

            return success;
        }


        /// <summary>
        /// Formats the URI for the request
        /// </summary>
        /// <param name="endpoint">The endpoint of the request.</param>
        /// <returns>URI string</returns>
        private string GetURI(string endpoint)
        {
            return String.Format("{0}://{1}/v{2}/{3}", this.Protocol, this.GatewayAddress, this.Version, endpoint);
        }

        /// <summary>
        /// Creates a new web request client.
        /// </summary>
        /// <param name="endpoint">The end point of the request.</param>
        /// <returns>HttpWebRequest</returns>
        private HttpWebRequest GetClient(string endpoint)
        {
            var client = (HttpWebRequest)System.Net.WebRequest.Create(this.GetURI(endpoint));
            client.Credentials = new System.Net.NetworkCredential(this.Username, this.Token);
            client.Headers.Add("User-Agent", String.Format("Official .NET {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            client.PreAuthenticate = true;

            return client;
        }

        /// <summary>
        /// Handles a WebException - if the exception is a 401 it raises an Auth exception, otherwise it tries to parse the error response.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="requestType">The request type</param>
        /// <returns>Response or raises exception</returns>
        private Response HandleException(WebException ex, string requestType)
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

            switch(requestType) {
                case "Purchase":
                default:
                    return Response.ParsePurchase(jsonResponse);
                case "Tokenize":
                    return Response.ParseTokenized(jsonResponse);
                case "Refund":
                    return Response.ParseRefund(jsonResponse);
                case "Plan":
                    return Response.ParsePlan(jsonResponse);
                case "Customer":
                    return Response.ParseCustomer(jsonResponse);
                case "Subscription":
                    return Response.ParseSubscription(jsonResponse);
            }
        }
    }
}
