using System.Collections.Generic;

namespace OpsGenieApi
{
    public class ApiListResponse : IApiResponse
    { 
        public List<Alert> Alerts { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public bool Ok { get; set; }
    }
}