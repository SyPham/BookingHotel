using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Catalog;
using BookingHotel.Web.ClassHelpers;

namespace BookingHotel.Web.ViewComponents
{
    [ViewComponent]
    public class vcProductRelated : ViewComponent
    {

        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion
        #region Ctor
        public vcProductRelated(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");

            _logging = logging;
            _config = config;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync(int id, int catId, int NumberItem)
        {
            var response = await client.GetAsync($"/api/product/getrelated?id={id}&catId={catId}&NumberItem={NumberItem}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<ProductModel> data = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            return View(data);
        }
    }
}
