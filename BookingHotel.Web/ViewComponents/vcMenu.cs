using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;
using BookingHotel.Web.Models;

namespace BookingHotel.Web.ViewComponents
{
    [ViewComponent]
    public class vcMenu : ViewComponent
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public vcMenu(IHttpClientFactory clientFactory, ILogging logging)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var responseProductCategory = await client.GetAsync("api/ProductCategory/getallstatus?status=true");
            var responseServiceCategory = await client.GetAsync("api/ServiceCategory/getallstatus?status=true");
            var responseArticleCategory = await client.GetAsync("api/ArticleCategory/getallstatus?status=true");
            var response = await client.GetAsync($"api/abount/getfirst");

            if (responseProductCategory == null 
                || responseServiceCategory == null 
                || responseArticleCategory == null
                || response == null
                )
                return View();

            if (responseProductCategory.StatusCode != System.Net.HttpStatusCode.OK ||
                response.StatusCode != System.Net.HttpStatusCode.OK ||
                responseServiceCategory.StatusCode != System.Net.HttpStatusCode.OK ||
                responseArticleCategory.StatusCode != System.Net.HttpStatusCode.OK
                )
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            AboutModel data = JsonConvert.DeserializeObject<AboutModel>(json);

            string jsonProductCategory = responseProductCategory.Content.ReadAsStringAsync().Result;
            List<ProductCategoryModel> dataProductCategory = JsonConvert.DeserializeObject<List<ProductCategoryModel>>(jsonProductCategory);


            string jsonerviceCategory = responseServiceCategory.Content.ReadAsStringAsync().Result;
            List<ServiceCategoryModel> dataServiceCategory = JsonConvert.DeserializeObject<List<ServiceCategoryModel>>(jsonerviceCategory);


            string jsonArticleCategory = responseArticleCategory.Content.ReadAsStringAsync().Result;
            List<ArticleCategoryModel> dataArticleCategory = JsonConvert.DeserializeObject<List<ArticleCategoryModel>>(jsonArticleCategory);

            var result = new MenuModel(dataProductCategory, dataServiceCategory, dataArticleCategory, data);

        

            return View(result);
        }
    }
}
