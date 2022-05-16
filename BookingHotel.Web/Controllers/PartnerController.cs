using Microsoft.AspNetCore.Mvc;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using BookingHotel.Model.Catalog;
using System.Net.Http;
using Microsoft.Extensions.Options;
using BookingHotel.Web.ClassHelpers;
using System.Security.Claims;
using static BookingHotel.Web.TagHelpers.MessageTagHelper;
using System;
using System.Text;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Lite;
using System.Net.Http.Headers;
using BookingHotel.Data.Enums;
using BookingHotel.Common;
using System.Collections.Generic;
using BookingHotel.Web.Models;

namespace BookingHotel.Web.Controllers
{
    public class PartnerController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public PartnerController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion
        [Route("thong-tin-doi-tac", Name = "partner-profile")]
        public async Task<IActionResult> Profile()
        {
            var response = await client.GetAsync($"api/partner/getbyaccount");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            PartnerModel data = JsonConvert.DeserializeObject<PartnerModel>(json);
            var IsVerified = User.Claims.FirstOrDefault(x => x.Type.Equals("IsVerified")).Value;
            var JwtToken = User.Claims.FirstOrDefault(x => x.Type.Equals("JwtToken")).Value;
            ViewBag.IsVerified = data.Account.IsVerified;
            ViewBag.JwtToken = JwtToken;
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.UploadUrl = _config.Value.ApiBase + "api/partnerv2/uploadavatar";
            return View(data);
        }
        [HttpPost]
        [Route("/thong-tin-doi-tac", Name = "partner-profile")]
        public async Task<IActionResult> Profile(PartnerModel item)
        {

            #region Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập tiêu đề";
                return View();
            }
            if (string.IsNullOrWhiteSpace(item.Representative))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập họ tên";
                return View();
            }

            if (string.IsNullOrWhiteSpace(item.Email) && IsValidEmail(item.Email))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập email và phải đúng định dạng email";
                return View();
            }
            if (item.Phone.IsPhoneNumber())
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập số điện thoại và phải đúng định dạng số điện thoại";
                return View();
            }
            if (string.IsNullOrWhiteSpace(item.Content))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập nội dung";
                return View();
            }

            //if (string.IsNullOrWhiteSpace(item.Address))
            //{
            //    ViewBag.Type = MessageType.danger;
            //    ViewBag.Content = "Vui lòng nhập địa chỉ";
            //    return View();
            //}
            #endregion


            //Ép kiểu json(Bước xử lý trung gian)
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PutAsync("api/partner/update2", stringContent); //Gửi lên server Put async

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                ProcessingResult data = JsonConvert.DeserializeObject<ProcessingResult>(jsonResponse);
                if (data.MessageType == MessageTypeEnum.Danger)
                {
                    ViewBag.Type = MessageType.danger;
                    ViewBag.Content = data.Message;
                    return View();
                }
                else
                {
                    //Hiện thông báo lỗi
                    #region Hiển thị thông báo
                    ViewBag.Type = MessageType.danger;
                    ViewBag.Content = "Đã xảy ra lỗi. Vui lòng thử lại";
                    #endregion
                    return View();
                }

            }
            else
            {
                //Hiện chúc mừng

                #region Hiển thị thông báo
                ViewBag.Type = MessageType.success;
                ViewBag.Content = "Đã cập nhật thành công.";
                #endregion

                //Xóa trắng form
                ModelState.Clear();
                var response2 = await client.GetAsync($"api/partner/getbyaccount");

                string json2 = response2.Content.ReadAsStringAsync().Result;
                PartnerModel data2 = JsonConvert.DeserializeObject<PartnerModel>(json2);
                var IsVerified = User.Claims.FirstOrDefault(x => x.Type.Equals("IsVerified")).Value;
                var JwtToken = User.Claims.FirstOrDefault(x => x.Type.Equals("JwtToken")).Value;
                ViewBag.IsVerified = data2.Account.IsVerified;
                ViewBag.JwtToken = JwtToken;
                ViewBag.Url = _config.Value.ApiUrl;
                ViewBag.UploadUrl = _config.Value.ApiBase + "api/partnerv2/uploadavatar";
                return View(data2);

            }
        }
        [Route("/doi-tac/danh-sach-san-pham", Name = "product-list-partner")]
        public async Task<IActionResult> ProductList()
        {
            var response = await client.GetAsync($"api/partner/getallproductbyaccount");
            var response2 = await client.GetAsync($"api/partner/getallproductissalebyaccount");
            var response3 = await client.GetAsync($"api/partner/getallproductstopbyaccount");

            if (response == null || response2 == null || response3 == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK
                || response2.StatusCode != System.Net.HttpStatusCode.OK
                || response3.StatusCode != System.Net.HttpStatusCode.OK
                )
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            List<ProductModel> data = JsonConvert.DeserializeObject<List<ProductModel>>(json);

            string json2 = response2.Content.ReadAsStringAsync().Result;
            List<ProductModel> data2 = JsonConvert.DeserializeObject<List<ProductModel>>(json2);

            string json3 = response.Content.ReadAsStringAsync().Result;
            List<ProductModel> data3 = JsonConvert.DeserializeObject<List<ProductModel>>(json3);

            var result = new ProductListViewModel(data, data2, data3);
            ViewBag.Url = _config.Value.ApiUrl;

            return View(result);
        }
     
        [Route("/doi-tac/chi-tiet/{id?}/{catid?}/{title?}", Name = "partner-detail")]
        public async Task<IActionResult> Detail(int ID = 0, int catID = 1, string title = "")
        {
            await Task.CompletedTask;
            return View();
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
