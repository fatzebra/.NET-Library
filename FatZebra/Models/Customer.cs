using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Customer : IRecord
    {
        private bool newRecord = true;

        /// <summary>
        /// The customer ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Indicates if the customer was created successfully
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// The customers email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The customers reference
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The customers first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The customers last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The customers full name
        /// </summary>
        public string CustomerName
        {
            get
            {
                return String.Join(" ", this.FirstName, this.LastName);
            }
        }

        /// <summary>
        /// The customers credit card token
        /// </summary>
        public string CardToken { get; set; }

        /// <summary>
        /// Instantiates a new Customer
        /// </summary>
        /// <param name="json">Raw JSON data</param>
        /// <returns>Customer</returns>
        public static Customer Parse(JsonValue json)
        {
            var customer = new Customer();

            if (json.ContainsKey("first_name") && json["first_name"] != null)
                customer.FirstName = json["first_name"].ReadAs<string>();
            
            if (json.ContainsKey("last_name") && json["last_name"] != null)
                customer.LastName = json["last_name"].ReadAs<string>();

            if (json.ContainsKey("id") && json["id"] != null)
                customer.ID = json["id"].ReadAs<string>();

            customer.Successful = customer.ID != null;
            
            if (json.ContainsKey("email") && json["email"] != null)
                customer.Email = json["email"].ReadAs<string>();

            if (json.ContainsKey("reference") && json["reference"] != null)
                customer.Reference = json["reference"].ReadAs<string>();

            if (json.ContainsKey("card_token") && json["card_token"] != null)
                customer.CardToken = json["card_token"].ReadAs<string>();

            return customer;
        }

        /// <summary>
        /// Find a customer
        /// </summary>
        /// <param name="ID">The customer ID</param>
        /// <returns>Customer</returns>
        public static Customer Find(string ID)
        {
            var response = Gateway.Get(String.Format("customers/{0}.json", ID));
            var respBase = Response.ParseBase(response);

            if (respBase.Successful)
            {
                return Customer.Parse(response["response"]);
            }
            else
            {
                throw new Exception(String.Format("Error retrieving subscription: {0}", respBase.Errors));
            }
        }

        /// <summary>
        /// Updates the customers credit card
        /// </summary>
        /// <param name="card_holder">The card holders name</param>
        /// <param name="card_number">The card number</param>
        /// <param name="card_expiry">The card expiry</param>
        /// <param name="cvv">The CVV</param>
        /// <returns>Indication of success</returns>
        public bool UpdateCard(string card_holder, string card_number, DateTime card_expiry, string cvv)
        {
            var payload = new JsonObject();
            var card = new JsonObject();
            card.Add("card_holder", card_holder);
            card.Add("card_number", card_number);
            card.Add("expiry_date", card_expiry.ToString("MM/yyyy"));
            card.Add("cvv", cvv);
            payload.Add("card", card);

            var response = Gateway.Put(String.Format("customers/{0}.json", this.ID), payload);

            return response["successful"].ReadAs<bool>();
        }
    }
}
