using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public class Subscription : IRecord
    {
        /// <summary>
        /// The subscription ID
        /// </summary>
		[JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// Indication of the subscription was successful (created) or not
        /// </summary>
		[JsonProperty("successful")]
		public bool Successful { get; set; }
        /// <summary>
        /// Customer ID for the subscription
        /// </summary>
		[JsonProperty("customer_id")]
		public string CustomerID { get; set; }
        /// <summary>
        /// The Plan ID for the subscription
        /// </summary>
		[JsonProperty("plan_id")]
		public string PlanID { get; set; }
        /// <summary>
        /// Subscription billing frequency
        /// </summary>
		[JsonProperty("frequency")]
		public string Frequency { get; set; }
        /// <summary>
        /// Subscription Start Date
        /// </summary>
		[JsonProperty("start_date")]
		public DateTime StartDate { get; set; }
        /// <summary>
        /// Subscription End Date
        /// </summary>
		[JsonProperty("end_date")]
		public DateTime? EndDate { get; set; }
        /// <summary>
        /// Subscription Next Billing Date
        /// </summary>
		[JsonProperty("next_billing_date")]
		public DateTime NextBillingDate { get; set; }
        /// <summary>
        /// Subscription Reference
        /// </summary>
		[JsonProperty("reference")]
		public string Reference { get; set; }
        /// <summary>
        /// The Last Status of the Subscription
        /// </summary>
		[JsonProperty("last_status")]
		public string LastStatus { get; set; }
        /// <summary>
        /// Indicates if the subscription is active or not
        /// </summary>
		[JsonProperty("is_active")]
		public bool IsActive { get; set; }
        
        /// <summary>
        /// Fetch all subscriptions
        /// </summary>
        /// <returns>List  of all subscriptions</returns>
        public static List<Subscription> All()
        {
			var response = Gateway.Get<List<Subscription>>("subscriptions.json");

			if (response.Successful)
            {
				return response.Result;
            }
            else
            {
				throw new Exception(String.Format("Error from gateway: {0}", response.Errors));
            }
        }

        /// <summary>
        /// Find a customer
        /// </summary>
        /// <param name="ID">The customer ID</param>
        /// <returns>Subscription</returns>
        public static Subscription Find(string ID)
        {
			var response = Gateway.Get<Subscription>(String.Format("subscriptions/{0}.json", ID));

			if (response.Successful)
            {
				return response.Result;
            }
            else
            {
				throw new Exception(String.Format("Error retrieving subscription: {0}", response.Errors));
            }
        }

        /// <summary>
        /// Create a new subscription
        /// </summary>
        /// <param name="customer_id">The Customer ID or Reference</param>
        /// <param name="plan_id">The Plan ID or Reference</param>
        /// <param name="frequency">Subscription frequency (Daily, Weekly, Fortnightly, Monthly, Quarterly, Bi-Annually, Annually)</param>
        /// <param name="reference">Your reference</param>
        /// <param name="start_date">Subscription start date</param>
        /// <param name="is_active">Indicates if the subscription is active or not.</param>
        /// <returns>Response</returns>
		public static Response<Subscription> Create(string customer_id, string plan_id, string frequency, string reference, DateTime start_date, bool is_active)
        {
			var req = new Requests.Subscription {
				CustomerID = customer_id,
				PlanID = plan_id,
				Reference = reference,
				Frequency = (SubscriptionFrequency)Enum.Parse(typeof(SubscriptionFrequency), frequency),
				StartDate = start_date,
				IsActive = is_active,
				TestMode = Gateway.TestMode
			};

			return Gateway.Post<Subscription>("subscriptions.json", req);
        }

        /// <summary>
        /// Cancel a Subscription
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool Cancel()
        {
			var req = new Requests.Subscription {
				IsActive = false
			};
					
			var response = Gateway.Put<Subscription>(String.Format("subscriptions/{0}.json", this.ID), req);

			this.IsActive = response.Result.IsActive;

			return response.Successful;
        }

        /// <summary>
        /// Resume a cancelled/paused subscription
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool Resume()
        {
			var req = new Requests.Subscription {
				IsActive = true
			};

			var response = Gateway.Put<Subscription>(String.Format("subscriptions/{0}.json", this.ID), req);

			this.IsActive = response.Result.IsActive;

			return response.Successful;
        }
    }
}
