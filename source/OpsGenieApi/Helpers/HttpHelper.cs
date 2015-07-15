using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ServiceStack.Text;

namespace OpsGenieApi.Helpers
{
    class HttpHelper
    {
        public static string UserAgent = "OpsGenieCli";
        
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
                    sb.Append(string.Format("{0}={1}; ", c.Key, c.Value));
                }
                headers.Add(HttpRequestHeader.Cookie.ToString(), sb.ToString());
            }

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

        public string DownloadUrl(string url, string httpMethod, string acceptContentType,
                                  Dictionary<string, string> headers, Dictionary<string, string> cookies = null)
        {
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Method = httpMethod;
            webReq.Accept = acceptContentType;
            webReq.UserAgent = "UnitTest";

            if (cookies != null && cookies.Any())
            {
                var sb = new StringBuilder();
                foreach (var c in cookies)
                {
                    sb.Append(string.Format("{0}={1}; ", c.Key, c.Value));
                }
                headers.Add(HttpRequestHeader.Cookie.ToString(), sb.ToString());
            }

            foreach (var h in headers)
            {
                webReq.Headers.Add(h.Key, h.Value);
            }

            try
            {

                using (var webRes = webReq.GetResponse())
                {
                    var r = webRes.DownloadText();

                    if (r == String.Empty
                     && webRes is HttpWebResponse)
                    {
                        var hr = webRes as HttpWebResponse;
                        r = string.Format("{0},{1},{2}", hr.StatusCode, (int)hr.StatusCode, hr.StatusDescription);
                    }

                    return r;
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
            var s = string.Format("{0},{1},{2}", httpResponse.StatusCode, (int)httpResponse.StatusCode, httpResponse.StatusDescription);
            Trace.WriteLine(s);
            return s;
        }


        public static T SendJsonToUrl<T>(string url, string httpMethod, object postData,
                                           IDictionary<string, string> headers = null,
                                           string acceptContentType = "application/json",
                                           string sendContentType = "application/json")
        {
            var response = SendJsonToUrl(url, httpMethod, postData, headers, acceptContentType, sendContentType);

            if (!string.IsNullOrWhiteSpace(response))
            {
                return JsonSerializer.DeserializeFromString<T>(response);
            }

            return default(T);
        }

        public static string SendJsonToUrl(string url, string httpMethod, object postData,
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
            webReq.UserAgent = "UnitTest";

            if (acceptContentType != null)
                webReq.Accept = acceptContentType;

            if (postData != null)
            {
                try
                {

                    using (var writer = new StreamWriter(webReq.GetRequestStream()))
                    {
                        JsonSerializer.SerializeToWriter(postData, writer);
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
                    r = string.Format("{0},{1},{2}", hr.StatusCode, (int)hr.StatusCode, hr.StatusDescription);
                }

                return r;
            }
           
        }

        public IDictionary<string, string> GetBasicAuthHeader(string userName, string password)
        {
            return new Dictionary<string, string>(1)
                       {
                           {
                               "Authorization",
                               string.Format("Basic {0}",Convert.ToBase64String(Encoding.ASCII.GetBytes(HttpUtility.UrlEncode(userName) + ":" +HttpUtility.UrlEncode(password))))
                               }
                       };
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

            return JsonSerializer.DeserializeFromString<T>(response);
        }
    }
}
