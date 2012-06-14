using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Subscription : IRecord
    {
        /// <summary>
        /// The subscription ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Indication of the subscription was successful (created) or not
        /// </summary>
        public bool Successful { get; set; }
        /// <summary>
        /// Customer ID for the subscription
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// The Plan ID for the subscription
        /// </summary>
        public string PlanID { get; set; }
        /// <summary>
        /// Subscription billing frequency
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// Subscription Start Date
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Subscription End Date
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Subscription Next Billing Date
        /// </summary>
        public DateTime NextBillingDate { get; set; }
        /// <summary>
        /// Subscription Reference
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// The Last Status of the Subscription
        /// </summary>
        public string LastStatus { get; set; }
        /// <summary>
        /// Indicates if the subscription is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Parses a new subscription from the JSON response
        /// </summary>
        /// <param name="json">JSON Response</param>
        /// <returns>Subscription</returns>
        public static Subscription Parse(JsonValue json)
        {
            var sub = new Subscription();

            if (json.ContainsKey("id") && json["id"] != null)
                sub.ID = json["id"].ReadAs<string>();
            
            sub.Successful = sub.ID != null;

            if (json.ContainsKey("customer") && json["customer"] != null)
                sub.CustomerID = json["customer"].ReadAs<string>();

            if (json.ContainsKey("plan") && json["plan"] != null)
                sub.PlanID = json["plan"].ReadAs<string>();
            
            if (json.ContainsKey("frequency") && json["frequency"] != null)
                sub.Frequency = json["frequency"].ReadAs<string>();

            if (json.ContainsKey("reference") && json["reference"] != null)
                sub.Reference = json["reference"].ReadAs<string>();

            if (json.ContainsKey("is_active") && json["is_active"] != null)
                sub.IsActive = json["is_active"].ReadAs<bool>(false);

            if (json.ContainsKey("last_status") && json["last_status"] != null)
                sub.LastStatus = json["last_status"].ReadAs<string>();

            if (json.ContainsKey("start_date") && json["start_date"] != null)
                sub.StartDate = json["start_date"].ReadAs<DateTime>(DateTime.MinValue);

            if (json.ContainsKey("next_billing_date") && json["next_billing_date"] != null)
                sub.NextBillingDate = json["next_billing_date"].ReadAs<DateTime>(DateTime.MinValue);
            
            if (json.ContainsKey("end_date") && json["end_date"] != null)
                sub.EndDate = json["end_date"].ReadAs<DateTime>(DateTime.MinValue);

            return sub;
        }

        
        /// <summary>
        /// Fetch all subscriptions
        /// </summary>
        /// <returns>List  of all subscriptions</returns>
        public static List<Subscription> All()
        {
            var response = Gateway.Get("subscriptions.json");
            var respBase = Response.ParseBase(response);
            var subscriptions = new List<Subscription>();

            if (respBase.Successful)
            {
                foreach(var item in (JsonArray)response["response"]) 
                {
                    var sub = Subscription.Parse(item);
                    subscriptions.Add(sub);
                }
            }
            else
            {
                throw new Exception(String.Format("Error from gateway: {0}", respBase.Errors));
            }

            return subscriptions;
        }

        /// <summary>
        /// Find a customer
        /// </summary>
        /// <param name="ID">The customer ID</param>
        /// <returns>Subscription</returns>
        public static Subscription Find(string ID)
        {
            var response = Gateway.Get(String.Format("subscriptions/{0}.json", ID));
            var respBase = Response.ParseBase(response);

            if (respBase.Successful)
            {
                return Subscription.Parse(response["response"]);
            }
            else
            {
                throw new Exception(String.Format("Error retrieving subscription: {0}", respBase.Errors));
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
        public static Response Create(string customer_id, string plan_id, string frequency, string reference, DateTime start_date, bool is_active)
        {

            var payload = new JsonObject();
            payload.Add("customer", customer_id);
            payload.Add("plan", plan_id);
            payload.Add("reference", reference);
            payload.Add("is_active", is_active);
            payload.Add("start_date", start_date.ToString("yyyy-MM-dd"));
            payload.Add("frequency", frequency);
            payload.Add("test", Gateway.TestMode);

            return Response.ParseSubscription(Gateway.Post("subscriptions.json", payload));
        }

        /// <summary>
        /// Cancel a Subscription
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool Cancel()
        {
            var payload = new JsonObject();
            payload.Add("is_active", false);

            var response = Gateway.Put(String.Format("subscriptions/{0}.json", this.ID), payload);

            var success = response["successful"].ReadAs<bool>();
            this.IsActive = response["response"]["is_active"].ReadAs<bool>();

            return success;
        }

        /// <summary>
        /// Resume a cancelled/paused subscription
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool Resume()
        {
            var payload = new JsonObject();
            payload.Add("is_active", true);

            var response = Gateway.Put(String.Format("subscriptions/{0}.json", this.ID), payload);

            var success = response["successful"].ReadAs<bool>();
            this.IsActive = response["response"]["is_active"].ReadAs<bool>();

            return success;
        }
    }
}
