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

            plan.ID = json["id"].ReadAs<string>();
            plan.Successful = true;
            plan.Name = json["name"].ReadAs<string>();
            plan.Description = json["description"].ReadAs<string>();
            plan.Amount = json["amount"].ReadAs<int>(0);
            plan.Reference = json["reference"].ReadAs<string>();
            return plan;
        }
    }
}
