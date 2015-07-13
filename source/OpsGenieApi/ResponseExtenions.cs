using System;

namespace OpsGenieApi
{   
    internal static class ResponseExtensions
    {
        public static T ToErrorResponse<T>(this T response, Exception e)
             where T : IApiResponse
        {
            response.Status = "Exception: " + e;
            response.Ok = false;
            response.Code = "Exception";

            return response;
        }

        public static T ToErrorResponse<T>(this IApiResponse response)
                where T : IApiResponse, new()
        {
            if (response != null)
            {
                return new T
                {
                    Code = response.Code,
                    Ok = false,
                    Status = response.Status
                };
            }
            return new T
            {
                Code = "000",
                Ok = false,
                Status = "Empty or null response"
            };
        }

        public static bool IsOk(this IApiResponse response)
        {
            if (response != null
                && !string.IsNullOrWhiteSpace(response.Code)
                && (response.Code.Equals("200")
                    || response.Code.Equals("201")))
            {
                return true;
            }
            return false;
        }
    }
}
