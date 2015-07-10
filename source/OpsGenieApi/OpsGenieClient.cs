using System;
using System.Diagnostics;
using System.Linq;
using OpsGenieApi.Model;
using ServiceStack;

namespace OpsGenieApi
{
    public class OpsGenieClient
    {
        private readonly OpsGenieClientConfig _config;

        public OpsGenieClient(OpsGenieClientConfig config)
        {
            _config = config;
        }

        public ApiResponse Raise(Alert alert)
        {
            var client = new JsonServiceClient();

            try
            {
                var createAlert = new
                {
                    apiKey = _config.ApiKey,
                    message = alert.Message,
                    alias = alert.Alias,
                    //recipients = alert.,
                    source = alert.Source,
                    description = alert.Description,
                    //tags ="",
                    note = alert.Note,
                    //teams ="",
                };


                var response = client.Post<ApiResponse>(_config.ApiUrl, createAlert);
                
                Trace.WriteLine(response);

                if (response.IsOk())
                {
                    response.Ok = true;
                    response.Alert = new Alert
                    {
                        Alertid = response.AlertId
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

        public ApiResponse Acknowledge(Alert alert)
        {
            return new ApiResponse();
        }

        public ApiResponse Close(string alertId, string alias, string note)
        {
            var client = new JsonServiceClient();

            try
            {
                var closeAlert = new
                {
                    apiKey = _config.ApiKey,
                    id = alertId,
                    alias,
                    note,
                };


                var response = client.Post<ApiResponse>(_config.ApiUrl + "/close", closeAlert);

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
            var url = string.Format("{0}?apiKey={1}&status=open&limit={2}",
                _config.ApiUrl, _config.ApiKey, maxNumber);

            var client = new JsonServiceClient();

            try
            {
                var response = client.Get<ListResponse>(url);
                Trace.WriteLine(response);
                if (response != null)
                {
                    return new ApiListResponse
                    {
                        Ok = true,
                        Status = response.Status,
                        Alerts = response.alerts.Select(a=>
                            new Alert
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