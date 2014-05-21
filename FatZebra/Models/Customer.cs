using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public class Customer : IRecord
    {
        /// <summary>
        /// The customer ID
        /// </summary>
		[JsonProperty("id")]
		public string ID { get; set; }

        /// <summary>
        /// Indicates if the customer was created successfully
        /// </summary>
		[JsonProperty("successful")]
		public bool Successful { get; set; }

        /// <summary>
        /// The customers email address
        /// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }

        /// <summary>
        /// The customers reference
        /// </summary>
		[JsonProperty("reference")]
		public string Reference { get; set; }

        /// <summary>
        /// The customers first name
        /// </summary>
		[JsonProperty("first_name")]
		public string FirstName { get; set; }

        /// <summary>
        /// The customers last name
        /// </summary>
		[JsonProperty("last_name")]
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
		[JsonProperty("card_token")]
		public string CardToken { get; set; }

        /// <summary>
        /// Find a customer
        /// </summary>
        /// <param name="ID">The customer ID</param>
        /// <returns>Customer</returns>
        public static Customer Find(string ID)
        {
			var response = Gateway.Get<Customer>(String.Format("customers/{0}.json", ID));

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
        /// Create a new Customer
        /// </summary>
        /// <param name="first_name">The customers first name</param>
        /// <param name="last_name">The customers last name</param>
        /// <param name="reference">Your reference for the customer</param>
        /// <param name="email">The customer email address</param>
        /// <param name="card_holder">The card holders name</param>
        /// <param name="card_number">The card number</param>
        /// <param name="cvv">The CVV</param>
        /// <param name="expiry_date">The card expiry date</param>
        /// <returns>Response</returns>
		public static Response<Customer> Create(string first_name, string last_name, string reference, string email, string card_holder, string card_number, string cvv, DateTime expiry_date)
        {
			var req = new Requests.Customer {
				FirstName = first_name,
				LastName = last_name,
				Email = email,
				Reference = reference,
				Card = new Requests.CreditCard {
					CardHolder = card_holder,
					CardNumber = card_number,
					SecurityCode = cvv,
					ExpiryDate = expiry_date
				},
				TestMode = Gateway.TestMode
			};



			return Gateway.Post<Customer>("customers.json", req);
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
			var req = new Requests.Customer {
				Card = new Requests.CreditCard {
				CardHolder = card_holder,
				CardNumber = card_number,
				ExpiryDate = card_expiry,
				SecurityCode = cvv
				}
			};
				
			var response = Gateway.Put<Customer>(String.Format("customers/{0}.json", this.ID), req);

			return response.Successful;
        }
    }
}
