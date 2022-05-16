
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
    public class vcMiniCart : ViewComponent
    {

        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion
        #region Ctor
        public vcMiniCart(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");

            _logging = logging;
            _config = config;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Url = _config.Value.ApiUrl;
            var response = await client.GetAsync($"api/cart/getcartitems");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<CartModel>>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.Total = data.Count();
            // Tính tổng giá
            decimal totalPrice = 0;
            foreach (var item in data)
            {
                var price = item.Quantity.Value * item.Product.Price.Value;
                totalPrice += price;

            }
            ViewBag.TotalPrice = totalPrice;

            return View(data);
        }
    }
}
