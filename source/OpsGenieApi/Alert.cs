using System.Collections.Generic;

namespace OpsGenieApi
{
    public class Alert
    {
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