using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Catalog;
using BookingHotel.Web.ClassHelpers;
using X.PagedList;

namespace BookingHotel.Web.Controllers
{
    public class ArticleController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public ArticleController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion

        [Route("/tin-tuc/{page?}", Name = "article-default")]
        [Route("/tin-tuc/{page?}/{catid?}/{title?}", Name = "article")]
        public async Task<IActionResult> Index(int? page, int catID = 0, string title = "")
        {  //Khai báo pageSize dùng chung
            int pageNumper = page ?? 1;
            //Khai báo url đến api cần gọi
            string apiUrl = $"api/article/getallstatus?status=true";
            if (catID > 0)
            {
                apiUrl = $"api/article/getallbycategory?catId={catID}";
            }

            var response = await client.GetAsync(apiUrl);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<ArticleModel> dataTemp = JsonConvert.DeserializeObject<List<ArticleModel>>(json);
            IPagedList<ArticleModel> data = dataTemp.ToPagedList(pageNumper, 12);
            ViewBag.Url = _config.Value.ApiUrl;
            if (catID > 0) {
                ViewBag.CatID = catID;
                var responseArticleCategory = await client.GetAsync($"api/articlecategory/getbyid?id={catID}");

                if (responseArticleCategory == null)
                    return View();

                if (responseArticleCategory.StatusCode != System.Net.HttpStatusCode.OK)
                    return View();

                string jsonArticleCategory = responseArticleCategory.Content.ReadAsStringAsync().Result;
                ArticleCategoryModel dataArticleCategory = JsonConvert.DeserializeObject<ArticleCategoryModel>(jsonArticleCategory);
                ViewBag.ArticleCategory = dataArticleCategory;
            }
          

            return View(data);
        }
        [Route("/tin-tuc/chi-tiet/{id?}/{catid?}/{title?}", Name = "article-detail")]
        public async Task<IActionResult> Detail(int ID = 0, int catID = 1, string title = "")
        {
            var response = await client.GetAsync($"api/article/getbyid?id={ID}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            ArticleModel data = JsonConvert.DeserializeObject<ArticleModel>(json);
            ViewBag.Url =_config.Value.ApiUrl;
            ViewBag.ID = ID;
            ViewBag.CatID = catID;
            return View(data);
        }
    }
}
