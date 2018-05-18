using System.Collections.Generic;

namespace OpsGenieApi.Model
{
    /// <summary>
    /// https://docs.opsgenie.com/docs/alert-api#section-create-alert
    /// </summary>
    public class Alert
    {
        public Alert()
        {
            Teams = new List<string>();
            Recipients = new List<string>();
            Actions = new List<string>();
            Tags = new List<string>();
        }

        /// <summary>
        /// Alert message, limited to 130 chars
        /// </summary>
        public string Message { get; set; }  

        public string Description { get; set; }
        public string Source { get; set; }
        public string Entity { get; set; }
        public List<string> Teams { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Tags { get; set; }
        public string Alertid { get; set; }
        /// <summary>
        /// Used for alert deduplication. A user defined identifier for the alert and there can be only one alert with open status with the same alias. Provides ability to assign a known id and later use this id to perform additional actions such as log, close, attach for the same alert.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Additonal note
        /// </summary>
        public string Note { get; set; }

        public List<string> Recipients { get; set; }
    }
}