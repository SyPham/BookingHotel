using Microsoft.AspNetCore.Mvc;
using BookingHotel.ApplicationService.Loggings;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using BookingHotel.Model.Catalog;
using System.Net.Http;
using Microsoft.Extensions.Options;
using BookingHotel.Web.ClassHelpers;
using System.Security.Claims;
using static BookingHotel.Web.TagHelpers.MessageTagHelper;
using System.Text;
using BookingHotel.Data.Enums;
using BookingHotel.Common;
using System.Collections.Generic;
using BookingHotel.Web.Models;
using Microsoft.AspNetCore.Http;
using BookingHotel.Common.Helpers;

namespace BookingHotel.Web.Controllers
{
    public class CustomerController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient _client;
        #endregion

        #region Ctor
        public CustomerController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            _client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion

        [Route("/khach-hang", Name = "customer")]
        public async Task<IActionResult> Index()
        {
            var accountid = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            var response = await _client.GetAsync($"api/customer/getbyid?id={accountid}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            CustomerModel data = JsonConvert.DeserializeObject<CustomerModel>(json);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/cap-nhat-anh-dai-dien", Name = "customer-upload-avatar")]
        public async Task<IActionResult> UploadImage(IFormFile ImageFile)
        {
            // Check if the ImageFile object is not null
            if (ImageFile.Length < 0)
                return Redirect("/thong-tin-ca-nhan");

            return Redirect("/thong-tin-ca-nhan");

        }
        [Route("/thong-tin-ca-nhan", Name = "customer-profile")]
        public async Task<IActionResult> Profile()
        {
            var accountId = User.Identity.Name;
            var response = await _client.GetAsync($"api/customer/getbyid?id={accountId.ToInt()}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            CustomerModel data = JsonConvert.DeserializeObject<CustomerModel>(json);
            var IsVerified = User.Claims.FirstOrDefault(x => x.Type.Equals("IsVerified")).Value;
            var JwtToken = User.Claims.FirstOrDefault(x => x.Type.Equals("JwtToken")).Value;
            ViewBag.IsVerified = data.Account.IsVerified;
            ViewBag.JwtToken = JwtToken;
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.UploadUrl = _config.Value.ApiBase + "api/customerv2/uploadavatar";
            return View(data);
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
        [HttpPost]
        [Route("/thong-tin-ca-nhan", Name = "customer-profile")]
        public async Task<IActionResult> Profile(CustomerModel item)
        {
            #region Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(item.FullName))
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
            if (string.IsNullOrWhiteSpace(item.Birthday))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập ngày sinh";
                return View();
            }

            if (string.IsNullOrWhiteSpace(item.Address))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập địa chỉ";
                return View();
            }
            #endregion


            //Ép kiểu json(Bước xử lý trung gian)
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PutAsync("api/customer/update2", stringContent); //Gửi lên server Put async

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
                } else
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
              
                var accountId = User.Identity.Name;
                var response2 = await _client.GetAsync($"api/customer/getbyid?id={accountId.ToInt()}");

                string json2 = response2.Content.ReadAsStringAsync().Result;
                CustomerModel data2 = JsonConvert.DeserializeObject<CustomerModel>(json2);
                var IsVerified = User.Claims.FirstOrDefault(x => x.Type.Equals("IsVerified")).Value;
                var JwtToken = User.Claims.FirstOrDefault(x => x.Type.Equals("JwtToken")).Value;
                ViewBag.IsVerified = data2.Account.IsVerified;
                ViewBag.JwtToken = JwtToken;
                ViewBag.Url = _config.Value.ApiUrl;
                ViewBag.UploadUrl = _config.Value.ApiBase + "api/customerv2/uploadavatar";
                return View(data2);
               // return RedirectToAction(nameof(Profile));


            }
        }
            [Route("/ma-giam-gia-cua-ban", Name = "customer-sale")]
        public async Task<IActionResult> Sale()
        {
            var response = await _client.GetAsync($"api/product/getallproductwallet");
            var response2 = await _client.GetAsync($"api/product/getallproductwalletisuse");
            var response3 = await _client.GetAsync($"api/product/getallproductwalletnotuse");

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

            var result = new ProductWalletViewModel(data, data2, data3);
            ViewBag.Url = _config.Value.ApiUrl;

            return View(result);
        }
        [Route("/danh-sach-yeu-thich-cua-ban", Name = "customer-wishlist")]
        public async Task<IActionResult> Wishlist()
        {
            var response = await _client.GetAsync($"api/product/getwishlist");

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
