using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
	public class Response<T>
    {
//        internal bool successful = false;
        internal IList<String> errors = new List<string>();
		internal IList<String> fraudCheckMessages = new List<string>();
//		internal T result = default(T);
//        internal bool test = false;
//        internal int records = 0;
//        internal int total_records = 0;
//        internal int page = 0;
//        internal int total_pages = 0;

		// {"successful":true,"response":{"authorization":1400645846,"id":"071-P-KCE1UEL7","card_number":"512345XXXXXX2346","card_holder":"M Smith","card_expiry":"2015-05-31","card_token":"hhyzk1va","amount":120,"decimal_amount":1.2,"successful":true,"message":"Approved","reference":"5f57a4d4-494e-419c-86f1-b97af1ac3ca6","currency":"AUD","transaction_id":"071-P-KCE1UEL7","settlement_date":"2014-05-22","transaction_date":"2014-05-21T14:17:26+10:00","response_code":"00","captured":true,"captured_amount":120},"errors":[],"test":true}

        /// <summary>
        /// Indicates if the request was successful or not
        /// </summary>
		[JsonProperty("successful")]
		public Boolean Successful { get; set; }

        /// <summary>
        /// Errors for the request
        /// </summary>
		[JsonProperty("errors")]
		public IList<string> Errors { get; set; }

		[JsonProperty("fraud_messages")]
		public IList<string> FraudMessages { get; set; } 

        /// <summary>
        /// The result object
        /// </summary>
		[JsonProperty("response")]
		public T Result { get; set; }

        /// <summary>
        /// Indicates if the request was in test mode or live.
        /// </summary>
		[JsonProperty("test")]
		public bool IsTest { get; set; }

        /// <summary>
        /// The number of records in this response
        /// </summary>
		[JsonProperty("records")]
		public int Records { get; set; }

        /// <summary>
        /// The total number of records in the response
        /// </summary>
		[JsonProperty("total_records")]
		public int TotalRecords { get; set; }

        /// <summary>
        /// The page number of the current response
        /// </summary>
		[JsonProperty("pages")]
		public int Page { get; set; }

        /// <summary>
        /// Number of pages in the results
        /// </summary>
		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }
    }
}
