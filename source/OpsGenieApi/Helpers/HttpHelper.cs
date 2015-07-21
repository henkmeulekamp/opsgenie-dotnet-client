using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace OpsGenieApi.Helpers
{
    class HttpHelper
    {
        private readonly IJsonSerializer _serializer;
        public static string UserAgent = "OpsGenieCli";

        public HttpHelper(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public string DownloadUrl(string url, string acceptContentType,
            IDictionary<string, string> headers,
            Dictionary<string, string> cookies = null)
        {
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Accept = acceptContentType;
            webReq.UserAgent = HttpHelper.UserAgent;

            if (cookies != null && cookies.Any())
            {
                var sb = new StringBuilder();
                foreach (var c in cookies)
                {
                    sb.Append($"{c.Key}={c.Value}; ");
                }
                headers.Add(HttpRequestHeader.Cookie.ToString(), sb.ToString());
            }

            if(headers!=null)
            foreach (var h in headers)
            {
                webReq.Headers.Add(h.Key, h.Value);
            }
            try
            {

                using (var webRes = webReq.GetResponse())
                {
                    return webRes.DownloadText();
                }

            }
            catch (WebException wex)
            {
                return ExtractStatus(wex);
            }
        }

        private static string ExtractStatus(WebException wex)
        {
            var httpResponse = wex.Response as HttpWebResponse;

            if (httpResponse == null) return wex.ToString();

            var s = $"{httpResponse.StatusCode},{(int) httpResponse.StatusCode},{httpResponse.StatusDescription}";
            Trace.WriteLine(s);
            return s;
        }


        public T SendJsonToUrl<T>(string url, string httpMethod, object postData,
                                           IDictionary<string, string> headers = null,
                                           string acceptContentType = "application/json",
                                           string sendContentType = "application/json")
        {
            var response = SendJsonToUrl(url, httpMethod, postData, headers, acceptContentType, sendContentType);

            if (!string.IsNullOrWhiteSpace(response))
            {
                return _serializer.DeserializeFromString<T>(response);
            }

            return default(T);
        }

        public string SendJsonToUrl(string url, string httpMethod, object postData,
                                       IDictionary<string, string> headers = null,
                                       string acceptContentType = "application/json",
                                       string sendContentType = "application/json")
        {

            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Method = httpMethod;

            if (headers != null)
            {
                foreach (var h in headers)
                {
                    webReq.Headers.Add(h.Key, h.Value);
                }
            }
            webReq.ContentType = sendContentType;
            webReq.UserAgent = UserAgent;

            if (acceptContentType != null)
                webReq.Accept = acceptContentType;

            if (postData != null)
            {
                try
                {
                    var encoding = new UTF8Encoding();
                    var byte1 = encoding.GetBytes(_serializer.SerializeToString(postData));

                    // Set the content length of the string being posted.
                    webReq.ContentLength = byte1.Length;

                    using (var stream = webReq.GetRequestStream())
                    {
                        stream.Write(byte1, 0, byte1.Length);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error sending Request: " + ex);
                    throw;
                }
            }
            using (var webRes = webReq.GetResponse())
            {
                var r = webRes.DownloadText();

                if (r == String.Empty
                    && webRes is HttpWebResponse)
                {
                    var hr = webRes as HttpWebResponse;
                    r = $"{hr.StatusCode},{(int) hr.StatusCode},{hr.StatusDescription}";
                }

                return r;
            }
           
        }

        /// <summary>
        /// compatibility with JsonServiceClient package 4
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Post<T>(string url, object obj)
        {
            return SendJsonToUrl<T>(url, "POST", obj);
        }

        public T Get<T>(string url)
        {
            var response = DownloadUrl(url, "applicaiton/json", null);

            return _serializer.DeserializeFromString<T>(response);
        }
    }
}
