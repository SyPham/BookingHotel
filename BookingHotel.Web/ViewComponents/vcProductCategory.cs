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
using BookingHotel.Common.Helpers;

namespace BookingHotel.Web.ViewComponents
{
    [ViewComponent]
    public class vcProductCategory : ViewComponent
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion
        #region Ctor
        public vcProductCategory(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");

            _logging = logging;
            _config = config;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await client.GetAsync($"api/productcategory/getmenus");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<HierarchyNode<MainMenuModel>>>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            return View(data);
        }
    }
}
