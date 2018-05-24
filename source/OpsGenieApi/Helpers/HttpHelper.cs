using System.Net.Http;
using System.Net.Http.Headers;

namespace OpsGenieApi.Helpers
{
    internal class HttpHelper
    {
        public static string UserAgent = "OpsGenieCli";

        public HttpHelper(string apiKey)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("GenieKey", apiKey);
        }

        public HttpClient Client { get; }
    }

}
