using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;
using BookingHotel.Web.ClassHelpers;

namespace BookingHotel.Web.ViewComponents
{
    [ViewComponent]
    public class vcProfileLeftPartner : ViewComponent
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion
        #region Ctor
        public vcProfileLeftPartner(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");

            _logging = logging;
            _config = config;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await client.GetAsync($"api/partner/getbyaccount");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            PartnerModel data = JsonConvert.DeserializeObject<PartnerModel>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            return View(data);
        }
    }
}
