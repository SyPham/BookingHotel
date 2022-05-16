using Microsoft.AspNetCore.Mvc;
using BookingHotel.ApplicationService.Loggings;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using BookingHotel.Model.Catalog;
using static BookingHotel.Web.TagHelpers.MessageTagHelper;
using System;

namespace BookingHotel.Web.Controllers
{
    public class ContactController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public ContactController(IHttpClientFactory clientFactory, ILogging logging)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
        }
        #endregion

        [Route("/lien-he", Name = "contact")]
        public async Task<IActionResult> Index()
        {

            var response = await client.GetAsync($"api/abount/getfirst");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            AboutModel data = JsonConvert.DeserializeObject<AboutModel>(json);
            ViewBag.About = data;
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
        [HttpPost]
        [Route("/lien-he", Name = "contact")]
        public async Task<IActionResult> Index(ContactModel item)
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
            if (string.IsNullOrWhiteSpace(item.Subject))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập tiêu đề";
                return View();
            }
            if (string.IsNullOrWhiteSpace(item.Mobi))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập số điện thoại";
                return View();
            }

            if (string.IsNullOrWhiteSpace(item.Content))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập nội dung liên hệ";
                return View();
            }
            #endregion

            #region Nhập thêm các cột trên view không có
            item.Status = false;
            item.CreateTime = DateTime.Now;
            #endregion

            //Ép kiểu json(Bước xử lý trung gian)
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("api/contact/insert", stringContent); //Gửi lên server Post async

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //Hiện thông báo lỗi
                #region Hiển thị thông báo
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Đã xảy ra lỗi. Vui lòng thử lại";
                #endregion
                return View();
            }
            else
            {
                //Hiện chúc mừng

                #region Hiển thị thông báo
                ViewBag.Type = MessageType.success;
                ViewBag.Content = "Đã gửi thư liên hệ. Chúng tôi sớm phản hồi.";
                #endregion

                //Xóa trắng form
                ModelState.Clear();

                //Trả về kết quả
                return View();
            }
        }
    }
}
