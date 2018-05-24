using System;
using System.Collections.Generic;

namespace OpsGenieApi.Model
{
    // opsgenie api classes
    public class Responder
    {
        public string name { get; set; }
        public string type { get; set; }
    }
   
    public class Integration
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Report
    {
        public int ackTime { get; set; }
        public int closeTime { get; set; }
        public string acknowledgedBy { get; set; }
        public string closedBy { get; set; }
    }

    public class DataListNode
    {
        public string id { get; set; }
        public string tinyId { get; set; }
        public string alias { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public bool acknowledged { get; set; }
        public bool isSeen { get; set; }
        public List<string> tags { get; set; }
        public bool snoozed { get; set; }
        public DateTime snoozedUntil { get; set; }
        public int count { get; set; }
        public DateTime lastOccurredAt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string source { get; set; }
        public string owner { get; set; }
        public string priority { get; set; }
        public List<object> responders { get; set; }
        public Integration integration { get; set; }
        public Report report { get; set; }
    }

    public class Paging
    {
        public string next { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class ListResponse
    {
        public List<DataListNode> data { get; set; }
        public Paging paging { get; set; }
        public double took { get; set; }
        public string requestId { get; set; }
    }
}
