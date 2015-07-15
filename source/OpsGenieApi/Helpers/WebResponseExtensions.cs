using System.IO;
using System.Net;

namespace OpsGenieApi.Helpers
{
    public static class WebResponseExtensions
    {

        public static string DownloadText(this WebResponse response)
        {
            if (response == null) return null;

            using (var stream = response.GetResponseStream())
            {
                if (stream == null) return null;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
