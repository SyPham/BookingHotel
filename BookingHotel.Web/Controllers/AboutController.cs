using Microsoft.AspNetCore.Mvc;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using BookingHotel.Web.ClassHelpers;
using System.Net.Http;
using BookingHotel.Model.Catalog;
using Newtonsoft.Json;

namespace BookingHotel.Web.Controllers
{
    public class AboutController : Controller
    {

        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion
        #region Ctor
        public AboutController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");

            _logging = logging;
            _config = config;
        }
        #endregion
        [Route("/gioi-thieu", Name = "about")]
        public async Task<IActionResult> Index()
        {
            var response = await client.GetAsync($"api/abount/getfirst");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            AboutModel data = JsonConvert.DeserializeObject<AboutModel>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            return View(data);
        }

    }
}
