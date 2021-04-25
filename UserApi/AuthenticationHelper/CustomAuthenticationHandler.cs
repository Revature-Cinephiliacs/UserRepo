using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace UserAPI.AuthenticationHelper
{
    /// <summary>
    /// An authentication middleware class. <br/>
    /// The request passes through to this class HandleAuthenticateAsync method
    /// before going into any endpoint
    /// </summary>
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public IServiceProvider ServiceProvider { get; set; }

        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider)
            : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// The request passes through this method before reaching any endpoint. <br/>
        /// This Sends a request to the authenticationAPI to determine whether signed in or not.
        /// And to also get all the current user permission.
        /// </summary>
        /// <returns></returns>
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            var token = Helper.GetTokenFromRequest(Request);

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Token is null");
            }


            var response = await Helper.Sendrequest("", RestSharp.Method.GET, token);
            if (!response.IsSuccessful)
            {
                return AuthenticateResult.Fail($"Balancer not authorize token : for token={token}");
            }
            var respObj = JsonConvert.DeserializeObject<PermissionObj>(response.Content);


            var claims = new List<Claim>();
            respObj.permissions.ForEach(per =>
            {
                claims.Add(new Claim(per, "true"));
            });
            var identity = new ClaimsIdentity(claims, nameof(CustomAuthenticationHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }

    /// <summary>
    /// An object to recieve the permission object from AuthenticationAPI
    /// </summary>
    public class PermissionObj
    {
        public string access { get; set; }
        public List<string> permissions { get; set; }
    }
}