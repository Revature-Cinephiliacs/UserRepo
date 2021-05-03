using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;

namespace UserAPI.AuthenticationHelper
{
    static class Helper
    {
        private static string _authUrl = "";

        /// <summary>
        /// Extract token from request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>token</returns>
        public static string GetTokenFromRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
        }

        /// <summary>
        /// Send request to Auth0
        /// </summary>
        /// <param name="urlExtension"></param>
        /// <param name="method"></param>
        /// <param name="token"></param>
        /// <returns>The response of the request</returns>
        public async static Task<IRestResponse> Sendrequest(string urlExtension, Method method, string token, dynamic body = null)
        {
            if (Debugger.IsAttached)
            {
                Helper._authUrl = "https://localhost:5005/Authentication";
            }
            else
            {
                Helper._authUrl = "http://20.45.0.16/Authentication";
            }
            var client = new RestClient(_authUrl + urlExtension);
            client.Timeout = -1;
            var request = new RestRequest(method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", token);
            if (body != null)
            {
                request.AddParameter("application/json", JsonSerializer.Serialize(body), ParameterType.RequestBody);
            }
            IRestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine("response.Content");
            Console.WriteLine(response.Content);
            return response;
        }

    }
}