namespace OpsGenieApi
{
    public class OpsGenieClientConfig
    {
        public string ApiUrl => "https://api.opsgenie.com/v2/alerts";
        public string ApiKey { get; set; }
    }
}