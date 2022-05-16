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
using X.PagedList;

namespace BookingHotel.Web.Controllers
{
    public class ServiceController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public ServiceController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion
        [Route("/dich-vu/{page?}", Name = "service-default")]
        [Route("/dich-vu/{page?}/{catid?}/{title?}", Name = "service")]
        public async Task<IActionResult> Index(int? page, int catID = 0, string title = "")
        {
            //Khai báo pageSize dùng chung
            int pageNumper = 0;
            int.TryParse(page.ToString(), out pageNumper);
            if (pageNumper == 0)
                pageNumper = 1;
            //Khai báo url đến api cần gọi
            string apiUrl = $"api/service/getallstatus?status=true";

            var response = await client.GetAsync(apiUrl);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<ServiceModel> dataTemp = JsonConvert.DeserializeObject<List<ServiceModel>>(json);
            IPagedList<ServiceModel> data = dataTemp.ToPagedList(pageNumper, 12);
            ViewBag.Url = _config.Value.ApiUrl;
            var responseServiceCategory = await client.GetAsync($"api/servicecategory/getbyid?id={catID}");

            if (responseServiceCategory == null)
                return View();

            if (responseServiceCategory.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string jsonServiceCategory = responseServiceCategory.Content.ReadAsStringAsync().Result;
            ServiceCategoryModel dataServiceCategory = JsonConvert.DeserializeObject<ServiceCategoryModel>(jsonServiceCategory);
            ViewBag.ServiceCategory = dataServiceCategory;
            return View(data);
        }
        [Route("/dich-vu/chi-tiet/{id?}/{catid?}/{title?}", Name = "service-detail")]
        public async Task<IActionResult> Detail(int ID = 0, int catID = 0, string title = "")
        {
            var response = await client.GetAsync($"api/service/getbyid?id={ID}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            var response2 = await client.GetAsync($"api/servicecategory/getbyid?id={catID}");

            if (response2 == null)
                return View();

            if (response2.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string json2 = response2.Content.ReadAsStringAsync().Result;
            ServiceCategoryModel data2 = JsonConvert.DeserializeObject<ServiceCategoryModel>(json2);
            ViewBag.ServiceCategory = data2;

            string json = response.Content.ReadAsStringAsync().Result;
            ServiceModel data = JsonConvert.DeserializeObject<ServiceModel>(json);
            ViewBag.Url = _config.Value.ApiUrl;

            ViewBag.ID = ID;
            ViewBag.CatID = catID;
            return View(data);
        }
    }
}
