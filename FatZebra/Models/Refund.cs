using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Refund : IRecord
    {
        /// <summary>
        /// The Transaction ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Indicates the success of the refund
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// The Refund amount
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The authorization ID
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// Result Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The original transaction ID
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// Instantiates a new Refund object from a JSON response
        /// </summary>
        /// <param name="json">The JSON response</param>
        /// <returns>Refund</returns>
        public static Refund Parse(JsonValue json)
        {
            var obj = new Refund();


            if (json.ContainsKey("id") && json["id"] != null)
                obj.ID = json["id"].ReadAs<string>();
            if (json.ContainsKey("authorization") && json["authorization"] != null)
                obj.Authorization = json["authorization"].ReadAs<string>();
            if (json.ContainsKey("successful") && json["successful"] != null)
                obj.Successful = json["successful"].ReadAs<bool>();
            if (json.ContainsKey("amount") && json["amount"] != null)
                obj.Amount = json["amount"].ReadAs<int>();
            if (json.ContainsKey("message") && json["message"] != null)
                obj.Message = json["message"].ReadAs<string>();
            if (json.ContainsKey("transaction_id") && json["transaction_id"] != null)
                obj.TransactionID = json["transaction_id"].ReadAs<string>();

            return obj;
        }
    }
}
