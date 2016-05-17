using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public class DirectDebit : DirectEntry
    {
        public static Response<DirectDebit> Create(string bsb, string account_number, string account_name, decimal amount, string reference, string description, DateTime date)
        {
            var req = new Requests.DirectEntry
            {
                BSB = bsb,
                AccountNumber = account_number,
                AccountName = account_name,
                Amount = amount,
                Description = description,
                Reference = reference,
                Date = date,
                TestMode = Gateway.TestMode
            };

            return Gateway.Post<DirectDebit>("direct_debits.json", req);
        }

        /// <summary>
        /// Fetches a Direct Debit from the database
        /// If the record cannot be found Null is return.
        /// </summary>
        /// <param name='identifier'>
        /// The DD record ID
        /// </param>
        public static DirectDebit Find(string identifier)
        {
            var response = Gateway.Get<DirectDebit>(String.Format("direct_debits/{0}.json", identifier));
            var dd = response.Result;

            if (response.Successful)
            {
                return dd;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Deletes the DE record
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            return Gateway.Delete(String.Format("direct_debits/{0}.json", this.ID));
        }
    }
}
