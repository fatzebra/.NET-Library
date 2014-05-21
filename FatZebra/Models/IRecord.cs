using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatZebra
{
    public interface IRecord
    {
		[JsonProperty("id")] 
		string ID { get; }
		[JsonProperty("successful")]
		bool Successful { get; }
    }
}
