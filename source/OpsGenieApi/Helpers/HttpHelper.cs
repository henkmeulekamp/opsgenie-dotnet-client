using System.Net.Http;
using System.Net.Http.Headers;

namespace OpsGenieApi.Helpers
{
    class HttpHelper
    {
        private readonly HttpClient _client;

        private readonly string _apiKey;
        public static string UserAgent = "OpsGenieCli";

        public HttpHelper(string apiKey)
        {
            _apiKey = apiKey;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("GenieKey", _apiKey);
        }

        public HttpClient Client => _client;
        
    }

}
