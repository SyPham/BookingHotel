using Microsoft.AspNetCore.Mvc;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Options;
using BookingHotel.Web.ClassHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Web.Controllers
{
    public class FaqController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public FaqController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion

        [Route("/cau-hoi-thuong-gap", Name = "faq")]
        public async Task<IActionResult> Index()
        {
            //Khai báo url đến api cần gọi
            string apiUrl = $"api/article/getallstatus?status=true";

            var response = await client.GetAsync(apiUrl);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<FaqModel> data= JsonConvert.DeserializeObject<List<FaqModel>>(json);
            ViewBag.Url = _config.Value.ApiUrl;

            return View(data);
        }

    }
}
