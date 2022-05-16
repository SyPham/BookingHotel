using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;

namespace BookingHotel.Web.ClassHelpers
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public CustomCookieAuthenticationEvents(IHttpClientFactory clientFactory, ILogging logging)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
        }
        #endregion

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            // Look for the LastChanged claim.
            //var lastChanged = (from c in userPrincipal.Claims
            //                   where c.Type == "LastChanged"
            //                   select c.Value).FirstOrDefault();

            //if (string.IsNullOrEmpty(lastChanged) ||
            //    !_userRepository.ValidateLastChanged(lastChanged))
            //{
            //    context.RejectPrincipal();

            //    await context.HttpContext.SignOutAsync(
            //        CookieAuthenticationDefaults.AuthenticationScheme);
            //}
        }
    }
}
