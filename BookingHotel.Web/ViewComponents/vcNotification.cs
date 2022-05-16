using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Catalog;
using BookingHotel.Web.ClassHelpers;

namespace BookingHotel.Web.ViewComponents
{
    [ViewComponent]
    public class vcNotification : ViewComponent
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public vcNotification(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await client.GetAsync($"api/notificationaccount/getnotificationsbyaccount?status=true");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string json = response.Content.ReadAsStringAsync().Result;
            List<NotificationModel> data = JsonConvert.DeserializeObject<List<NotificationModel>>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.Total = data.Count();
            return View(data);
        }
       
    }
}
