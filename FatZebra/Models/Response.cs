using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;

namespace FatZebra
{
	public class Response<T>
    {
        /// <summary>
        /// Indicates if the request was successful or not
        /// </summary>
		[JsonProperty("successful")]
		public Boolean Successful { get; set; }

        /// <summary>
        /// Errors for the request
        /// </summary>
		[DefaultValue(default(List<String>))]
		[JsonProperty("errors")]
		public IList<string> Errors { get; set; }

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
