using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BookingHotel.Web.ClassHelpers
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var expat = await _accessor.HttpContext.GetTokenAsync("expires_at");
            var dataExp = DateTime.Parse(expat, null, DateTimeStyles.RoundtripKind);
            if ((dataExp - DateTime.Now).TotalMinutes < 10)
            {
                //SNIP GETTING A NEW TOKEN IF ITS ABOUT TO EXPIRE
            }

            var token = await _accessor.HttpContext.GetTokenAsync("access_token");

            // Use the token to make the call.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
