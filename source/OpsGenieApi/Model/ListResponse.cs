using System.Collections.Generic;
using System.Security;

namespace OpsGenieApi.Model
{
    internal class Alert
    {
        public string id { get; set; }
        public string alias { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public bool isSeen { get; set; }
        public bool acknowledged { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string tinyId { get; set; }
        public List<string> tags { get; set; }
    }

    internal class ListResponse : IApiResponse
    {
        public List<Alert> alerts { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public bool Ok { get; set; }
    }
}
