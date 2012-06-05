using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace FatZebra
{
    public class Subscription : IRecord
    {
        public string ID { get; set; }
        public bool Successful { get; set; }
        public string CustomerID { get; set; }
        public string PlanID { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime NextBillingDate { get; set; }
        public string Reference { get; set; }
        public string LastStatus { get; set; }
        public bool IsActive { get; set; }

        public static Subscription Parse(JsonValue json)
        {
            var sub = new Subscription();

            if (json.ContainsKey("id") && json["id"] != null)
                sub.ID = json["id"].ReadAs<string>();
            
            sub.Successful = sub.ID != null;

            if (json.ContainsKey("customer") && json["customer"] != null)
                sub.CustomerID = json["customer"].ReadAs<string>();

            if (json.ContainsKey("plan") && json["plan"] != null)
                sub.PlanID = json["plan"].ReadAs<string>();
            
            if (json.ContainsKey("frequency") && json["frequency"] != null)
                sub.Frequency = json["frequency"].ReadAs<string>();

            if (json.ContainsKey("reference") && json["reference"] != null)
                sub.Reference = json["reference"].ReadAs<string>();

            if (json.ContainsKey("is_active") && json["is_active"] != null)
                sub.IsActive = json["is_active"].ReadAs<bool>(false);

            if (json.ContainsKey("last_status") && json["last_status"] != null)
                sub.LastStatus = json["last_status"].ReadAs<string>();

            if (json.ContainsKey("start_date") && json["start_date"] != null)
                sub.StartDate = json["start_date"].ReadAs<DateTime>(DateTime.MinValue);

            if (json.ContainsKey("next_billing_date") && json["next_billing_date"] != null)
                sub.NextBillingDate = json["next_billing_date"].ReadAs<DateTime>(DateTime.MinValue);
            
            if (json.ContainsKey("end_date") && json["end_date"] != null)
                sub.EndDate = json["end_date"].ReadAs<DateTime>(DateTime.MinValue);

            return sub;
        }
    }
}
