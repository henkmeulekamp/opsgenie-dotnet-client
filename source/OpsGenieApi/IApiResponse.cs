namespace OpsGenieApi
{
    public interface IApiResponse
    {
        string Code { get; set; }
        string Status { get; set; }
        bool Ok { get; set; }    
    }
}