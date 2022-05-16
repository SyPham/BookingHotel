using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Catalog;
using BookingHotel.Web.ClassHelpers;
using BookingHotel.Common.Helpers;

namespace BookingHotel.Web.ViewComponents
{
    [ViewComponent]
    public class vcProfileLeft : ViewComponent
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion
        #region Ctor
        public vcProfileLeft(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");

            _logging = logging;
            _config = config;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //First get user claims    
           
            var accountId = User.Identity.Name;
            var response = await client.GetAsync($"api/customer/getbyid?id={accountId.ToInt()}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            CustomerModel data = JsonConvert.DeserializeObject<CustomerModel>(json);
            return View(data);
        }
    }
}
