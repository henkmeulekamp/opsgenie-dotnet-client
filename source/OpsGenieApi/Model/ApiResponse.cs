using System;

namespace OpsGenieApi.Model
{
    public class ApiResponse 
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public bool Ok { get; set; }
        public string AlertId { get; set; }

        //v2 properties
        public string result { get;set; }
        public string requestId { get; set; }
        public decimal took { get; set; }
    }


    public class ApiV2Response
    {
        //v2 properties
        public string result { get; set; }
        public string requestId { get; set; }
        public decimal took { get; set; }

        public Data data { get; set; }
    }

    public class Data
    {
        public bool success { get; set; }
        public string action { get; set; }
        public DateTime processedAt { get; set; }
        public string integrationId { get; set; }
        public bool isSuccess { get; set; }
        public string status { get; set; }
        public string alertId { get; set; }
        public string alias { get; set; }
    }

}