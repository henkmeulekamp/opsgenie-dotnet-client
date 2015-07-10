namespace OpsGenieApi
{
    public class ApiResponse : IApiResponse
    {
        public Alert Alert { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public bool Ok { get; set; }
        public string AlertId { get; set; }
    }
}