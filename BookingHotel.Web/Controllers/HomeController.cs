using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Catalog;
using BookingHotel.Web.ClassHelpers;

namespace BookingHotel.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public HomeController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion


        [Route("/", Name = "home")]
        public async Task<IActionResult> IndexAsync()
        {
            var responseProductCategory = await client.GetAsync($"api/productcategory/getallstatus?status=true");

            if (responseProductCategory == null)
                return View();

            if (responseProductCategory.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
   
            string jsonProductCategory = responseProductCategory.Content.ReadAsStringAsync().Result;
            List<ProductCategoryModel> dataProductCategory = JsonConvert.DeserializeObject<List<ProductCategoryModel>>(jsonProductCategory);
            ViewBag.ProductCategory = dataProductCategory.OrderByDescending(x=>x.Id).Take(5).ToList();
            return View(dataProductCategory);
        }
        [Route("Error", Name = "home-error")]
        public IActionResult Error()
        {
           
            return View();
        }

    }
}
