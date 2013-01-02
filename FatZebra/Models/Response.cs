using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Response
    {
        internal bool successful = false;
        internal IList<String> errors = new List<string>();
        internal IRecord result = null;
        internal bool test = false;
        internal int records = 0;
        internal int total_records = 0;
        internal int page = 0;
        internal int total_pages = 0;

        /// <summary>
        /// Indicates if the request was successful or not
        /// </summary>
        public bool Successful { get { return successful; } }

        /// <summary>
        /// Errors for the request
        /// </summary>
        public IList<string> Errors { get { return errors; } }

        /// <summary>
        /// The result object
        /// </summary>
        public IRecord Result { get { return result; } }

        /// <summary>
        /// Indicates if the request was in test mode or live.
        /// </summary>
        public bool IsTest { get { return test; } }

        /// <summary>
        /// The number of records in this response
        /// </summary>
        public int Records { get { return records; } }

        /// <summary>
        /// The total number of records in the response
        /// </summary>
        public int TotalRecords { get { return total_records; } }

        /// <summary>
        /// The page number of the current response
        /// </summary>
        public int Page { get { return page; } }

        /// <summary>
        /// Number of pages in the results
        /// </summary>
        public int TotalPages { get { return total_pages; } }

        /// <summary>
        /// Instantiates a new response for a Purchase transaction
        /// </summary>
        /// <param name="jsonResponse">The Raw JSON input</param>
        /// <returns>Response</returns>
        public static Response ParsePurchase(JsonValue response)
        {
            Response obj = ParseBase(response);

            obj.result = Purchase.Parse(response["response"]);

            return obj;
        }

        /// <summary>
        /// Instantiates a new base response object.
        /// </summary>
        /// <param name="response">The JSON input from the API calls</param>
        /// <returns>Response</returns>
        public static Response ParseBase (JsonValue response)
		{
			var obj = new Response ();
			try {
				obj.test = response ["test"].ReadAs<bool> (false);
				obj.successful = response ["successful"].ReadAs<bool> (false);

				// Optionals
				if (response.ContainsKey ("total_pages"))
					obj.total_pages = response ["total_pages"].ReadAs<int> (0);
				if (response.ContainsKey ("page"))
					obj.page = response ["page"].ReadAs<int> (0);
				if (response.ContainsKey ("total_records"))
					obj.total_records = response ["total_records"].ReadAs<int> (0);
				if (response.ContainsKey ("records"))
					obj.records = response ["records"].ReadAs<int> (0);

				foreach (var error in (JsonArray)response["errors"]) {
					obj.errors.Add (error.ReadAs<string> ());
				}
			} catch (Exception ex) {
				System.Diagnostics.Debugger.Log(1, "Parse", 
				                                String.Format("Exception caught attempting to parse response. {0}. Response content: {1}", 
				              ex.Message, 
				              response.ToString()));

				throw;
			}

            return obj;
        }

        /// <summary>
        /// Instantiates a new response for a TokenizeCard transaction
        /// </summary>
        /// <param name="jsonResponse">The Raw JSON input</param>
        /// <returns>Response</returns>
        public static Response ParseTokenized(JsonValue response)
        {
            var obj = ParseBase(response);

            obj.result = CreditCard.Parse(response["response"]);
            return obj;

        }

        /// <summary>
        /// Instantiates a new response for a Refund transaction
        /// </summary>
        /// <param name="jsonResponse">The Raw JSON input</param>
        /// <returns>Response</returns>
        public static Response ParseRefund(JsonValue response)
        {
            var obj = ParseBase(response);

            obj.result = Refund.Parse(response["response"]);
            return obj;
        }

        /// <summary>
        /// Instantiates a new response for a Plan
        /// </summary>
        /// <param name="jsonResponse">The Raw JSON input</param>
        /// <returns>Response</returns>
        public static Response ParsePlan(JsonValue response)
        {
            var obj = ParseBase(response);

            obj.result = Plan.Parse(response["response"]);

            return obj;
        }

        /// <summary>
        /// Instantiates a new response for a Customer
        /// </summary>
        /// <param name="jsonResponse">The Raw JSON input</param>
        /// <returns>Response</returns>
        public static Response ParseCustomer(JsonValue response)
        {
            var obj = ParseBase(response);

            obj.result = Customer.Parse(response["response"]);

            return obj;
        }

        /// <summary>
        /// Instantiates a new response for a Subscription
        /// </summary>
        /// <param name="jsonResponse">The Raw JSON input</param>
        /// <returns>Response</returns>
        public static Response ParseSubscription(JsonValue response)
        {
            var obj = ParseBase(response);

            obj.result = Subscription.Parse(response["response"]);

            return obj;
        }
    }
}
