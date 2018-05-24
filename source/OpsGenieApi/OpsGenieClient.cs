using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using OpsGenieApi.Helpers;
using OpsGenieApi.Model;

namespace OpsGenieApi
{
    // Implements alert api opsgenie
    // see API docs: https://docs.opsgenie.com/docs/api-overview
    public class OpsGenieClient
    {
        private readonly OpsGenieClientConfig _config;
        private readonly IJsonSerializer _serializer;
        private readonly HttpHelper _httpHelper;

        public OpsGenieClient(OpsGenieClientConfig config, IJsonSerializer serializer)
        {
            _config = config;
            _serializer = serializer;
            _httpHelper = new HttpHelper(config.ApiKey);
        }

        public ApiResponse Raise(Alert alert)
        {
           if (alert == null || string.IsNullOrWhiteSpace(alert.Message))
                throw new ArgumentException("Alert message is required", nameof(alert));

    
                var createAlert = new
                {
                    message = alert.Message,
                    alias = alert.Alias,
                    source = alert.Source,
                    description = alert.Description,
                    tags = alert.Tags,
                    note = alert.Note,
                    responders = CreateResponders(alert.Teams, alert.Recipients).ToArray()
                };

                var json = _serializer.SerializeToString(createAlert);

                Trace.WriteLine(json);

                var httpResponse = _httpHelper.Client.PostAsync(_config.ApiUrl,
                    new StringContent(json, Encoding.UTF8, "application/json")).Result;

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new ApiResponse
                    {
                        Code = ((int) httpResponse.StatusCode).ToString(),
                        Ok = false,
                        Status = httpResponse.ReasonPhrase
                    };
                }

                var response =
                    _serializer.DeserializeFromString<ApiResponse>(httpResponse.Content.ReadAsStringAsync().Result);
                response.Code = ((int) httpResponse.StatusCode).ToString();
                response.Ok = true;
                return response;
          
        }

        //https://api.opsgenie.com/v2/alerts/requests/:requestId

        public ApiV2Response GetStatus(string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentException("requestId is mandatory", nameof(requestId));

            try
            {

                var url = _config.ApiUrl + "/requests/" + requestId;

                Trace.WriteLine(url);

                var responseBody = _httpHelper.Client.GetStringAsync(url).Result;

                return _serializer.DeserializeFromString<ApiV2Response>(responseBody);
                
            }
            catch (Exception e)
            {
                return new ApiV2Response
                {
                    result = e.ToString(),
                    data = new Data
                    {
                        success = false
                    }
                };
            }
        }

        private static IEnumerable<Responder> CreateResponders(IEnumerable<string> alertTeams,
            IEnumerable<string> alertRecipients)
        {
            foreach (var team in alertTeams)
                yield return new Responder { name = team, type = "team"};

            foreach (var recipient in alertRecipients)
                yield return new Responder { name = recipient, type = "user"};
        }

     

        private bool AlertAction(string action, string alertId, string alias, string note)
        {
            var url = _config.ApiUrl + "/" + (
                          !string.IsNullOrEmpty(alertId)
                              ? alertId + "/" + action + "?identifierType=id"
                              : alias + "/" + action + "?identifierType=alias");


            var notePost = new
            {
                note = note ?? ""
            };

            var json = _serializer.SerializeToString(notePost);

            Trace.WriteLine(url + "\n" + json);

            var httpResponse = _httpHelper.Client.PostAsync(url,
                new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var responseData = httpResponse.Content.ReadAsStringAsync().Result;

            Trace.WriteLine(responseData);

            var resp = _serializer.DeserializeFromString<ApiV2Response>(responseData);

            return resp.requestId != null;
        }

        public bool Acknowledge(string alertId, string alias, string note)
        {
            return AlertAction("acknowledge", alertId, alias, note);
        }

        public bool UnAcknowledge(string alertId, string alias, string note)
        {
            return AlertAction("unacknowledge", alertId, alias, note);
        }

        public bool Close(string alertId, string alias, string note)
        {
            return AlertAction("close", alertId, alias, note);

        }

        public bool AddNote(string alertId, string alias, string note)
        {
            return AlertAction("notes", alertId, alias, note);

        }

        public ListResponse GetLastOpenAlerts(int maxNumber = 20)
        {
            var url = _config.ApiUrl;

            Trace.WriteLine(url);

            var responseBody = _httpHelper.Client.GetStringAsync(url).Result;

            return _serializer.DeserializeFromString<ListResponse>(responseBody);

        }
    }
}