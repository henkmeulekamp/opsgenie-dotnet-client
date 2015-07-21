using System;
using System.Diagnostics;
using System.Linq;
using OpsGenieApi.Helpers;
using OpsGenieApi.Model;

namespace OpsGenieApi
{
    // Implements alert api opsgenie
    // see https://www.opsgenie.com/docs/web-api/alert-api
    public class OpsGenieClient
    {
        private readonly OpsGenieClientConfig _config;        
        private readonly HttpHelper _httpHelper;

        public OpsGenieClient(OpsGenieClientConfig config, IJsonSerializer serializer)
        {
            _config = config;
            _httpHelper = new HttpHelper(serializer);

        }

        public ApiResponse Raise(Alert alert)
        {
            if(alert == null || string.IsNullOrWhiteSpace(alert.Message))
                throw new ArgumentException("Alert message is required", "alert");

            try
            {
                var createAlert = new
                {
                    apiKey = _config.ApiKey,
                    message = alert.Message,
                    alias = alert.Alias,
                    recipients = alert.Recipients,
                    source = alert.Source,
                    description = alert.Description,
                    tags = alert.Tags,
                    note = alert.Note,
                    teams = alert.Teams
                };


                var response = _httpHelper.Post<ApiResponse>(_config.ApiUrl, createAlert);
                
                Trace.WriteLine(response);

                if (response.IsOk())
                {
                    response.Ok = true;
                    response.Alert = new Alert
                    {
                        Alertid = response.AlertId //new alert id
                    };
                    return response;
                }
                return response.ToErrorResponse<ApiResponse>();
            }
            catch (Exception e)
            {
                return new ApiResponse().ToErrorResponse(e);
            }        
        }

        public ApiResponse Acknowledge(string alertId, string alias, string note)
        {
            try
            {
                var closeAlert = new
                {
                    apiKey = _config.ApiKey,
                    id = alertId,
                    alias,
                    note,
                };
                
                var response = _httpHelper.Post<ApiResponse>(_config.ApiUrl + "/acknowledge", closeAlert);

                Trace.WriteLine(response);

                if (response.IsOk())
                {
                    response.Ok = true;
                    return response;
                }
                return response.ToErrorResponse<ApiResponse>();
            }
            catch (Exception e)
            {
                return new ApiResponse().ToErrorResponse(e);
            }        

        }

        public ApiResponse Close(string alertId, string alias, string note)
        {

            try
            {
                var closeAlert = new
                {
                    apiKey = _config.ApiKey,
                    id = alertId,
                    alias,
                    note,
                };


                var response = _httpHelper.Post<ApiResponse>(_config.ApiUrl + "/close", closeAlert);

                Trace.WriteLine(response);

                if (response.IsOk())
                {                    
                    response.Ok = true;                 
                    return response;
                }
                return response.ToErrorResponse<ApiResponse>();
            }
            catch (Exception e)
            {
                return new ApiResponse().ToErrorResponse(e);
            }        

        }

        public ApiListResponse GetLastOpenAlerts(int maxNumber = 20)
        {
            var url = $"{_config.ApiUrl}?apiKey={_config.ApiKey}&status=open&limit={maxNumber}";

     
            try
            {
                var response = _httpHelper.Get<ListResponse>(url);
                Trace.WriteLine(response);
                if (response != null)
                {
                    return new ApiListResponse
                    {
                        Ok = true,
                        Status = response.Status,
                        Alerts = response.alerts.Select(a=>
                            new Alert //todo more properties
                            {
                                Alertid = a.id,
                                Message = a.message,
                                Alias = a.alias,                              
                            }
                            ).ToList()

                    };
                }
                return response.ToErrorResponse<ApiListResponse>();
            }
            catch (Exception e)
            {
                return new ApiListResponse().ToErrorResponse(e);
            }
        }

    }
}