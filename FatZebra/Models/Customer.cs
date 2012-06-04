using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Customer : IRecord
    {
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
            customer.FirstName = json["first_name"].ReadAs<string>();
            customer.LastName = json["last_name"].ReadAs<string>();
            customer.ID = json["id"].ReadAs<string>();
            customer.Successful = true;
            customer.Email = json["email"].ReadAs<string>();
            customer.Reference = json["reference"].ReadAs<string>();
            customer.CardToken = json["card_token"].ReadAs<string>();

            return customer;
        }
    }
}
