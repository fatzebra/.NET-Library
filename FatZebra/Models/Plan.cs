using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Plan : IRecord
    {
        /// <summary>
        /// The Plan ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Indicates if the plan was created successfully or not
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// The Plan Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Plan Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Plan Amount, as an integer
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The Plan Reference
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Creates a new Plan object based on the JSON returned.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Plan Parse(JsonValue json)
        {
            var plan = new Plan();

            if (json.ContainsKey("id") && json["id"] != null)
                plan.ID = json["id"].ReadAs<string>();
            
            plan.Successful = plan.ID != null;
            
            if (json.ContainsKey("name") && json["name"] != null)
                plan.Name = json["name"].ReadAs<string>();
            
            if (json.ContainsKey("description") && json["description"] != null)
                plan.Description = json["description"].ReadAs<string>();
            
            if (json.ContainsKey("amount") && json["amount"] != null)
                plan.Amount = json["amount"].ReadAs<int>(0);
            
            if (json.ContainsKey("reference") && json["reference"] != null)
                plan.Reference = json["reference"].ReadAs<string>();
            
            return plan;
        }

        /// <summary>
        /// Creates a new Plan
        /// </summary>
        /// <param name="name">The name of the plan</param>
        /// <param name="reference">The reference</param>
        /// <param name="description">The plan description</param>
        /// <param name="amount">The plan amount, as an integer</param>
        /// <returns>Response</returns>
        public static Response Create(string name, string reference, string description, int amount)
        {
            var payload = new JsonObject();
            payload.Add("name", name);
            payload.Add("description", description);
            payload.Add("reference", reference);
            payload.Add("amount", amount);
            payload.Add("test", Gateway.TestMode);

            return Response.ParsePlan(Gateway.Post("plans.json", payload));
        }

        /// <summary>
        /// Find a Plan
        /// </summary>
        /// <param name="ID">The Plan ID</param>
        /// <returns>Plan</returns>
        public static Plan Find(string ID)
        {
            var response = Gateway.Get(String.Format("plans/{0}.json", ID));
            var respBase = Response.ParseBase(response);

            if (respBase.Successful)
            {
                return Plan.Parse(response["response"]);
            }
            else
            {
                throw new Exception(String.Format("Error fetching plan: {0}", respBase.Errors));
            }
        }
    }
}
