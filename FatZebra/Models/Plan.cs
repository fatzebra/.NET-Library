using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public class Plan : IRecord
    {
        /// <summary>
        /// The Plan ID
        /// </summary>
		[JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Indicates if the plan was created successfully or not
        /// </summary>
		[JsonProperty("successful")]
        public bool Successful { get; set; }

        /// <summary>
        /// The Plan Name
        /// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

        /// <summary>
        /// The Plan Description
        /// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }

        /// <summary>
        /// The Plan Amount, as an integer
        /// </summary>
		[JsonProperty("amount")]
		public int Amount { get; set; }

        /// <summary>
        /// The Plan Reference
        /// </summary>
		[JsonProperty("reference")]
		public string Reference { get; set; }

        /// <summary>
        /// Creates a new Plan
        /// </summary>
        /// <param name="name">The name of the plan</param>
        /// <param name="reference">The reference</param>
        /// <param name="description">The plan description</param>
        /// <param name="amount">The plan amount, as an integer</param>
        /// <returns>Response</returns>
		public static Response<Plan> Create(string name, string reference, string description, int amount)
        {
			var req = new Requests.Plan {
				Name = name,
				Description = description,
				Reference = reference,
				Amount = amount,
				TestMode = Gateway.TestMode
			};

			return Gateway.Post<Plan>("plans.json", req);
        }

        /// <summary>
        /// Find a Plan
        /// </summary>
        /// <param name="ID">The Plan ID</param>
        /// <returns>Plan</returns>
        public static Plan Find(string ID)
        {
			var response = Gateway.Get<Plan>(String.Format("plans/{0}.json", ID));

			if (response.Successful)
            {
				return response.Result;
            }
            else
            {
				throw new Exception(String.Format("Error fetching plan: {0}", response.Errors));
            }
        }
    }
}
