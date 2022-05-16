using Microsoft.AspNetCore.Mvc;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using BookingHotel.Model.Auth;
using BookingHotel.Web.Models;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using static BookingHotel.Web.TagHelpers.MessageTagHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Enums;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace BookingHotel.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public AccountController(IHttpClientFactory clientFactory, ILogging logging)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
        }
        #endregion

        [Route("/tai-khoan", Name = "account")]
        public async Task<IActionResult> Index()
        {
            var id = User.Identity.Name.ToInt();
            var response = await client.GetAsync($"api/customer/getbyid?id={id}");
            var response2 = await client.GetAsync($"api/account/getbyid?id={id}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
                string json = response.Content.ReadAsStringAsync().Result;
            CustomerModel data = JsonConvert.DeserializeObject<CustomerModel>(json);

            if (response2 == null)
                return View();

            if (response2.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string json2 = response2.Content.ReadAsStringAsync().Result;
            AccountModel data2 = JsonConvert.DeserializeObject<AccountModel>(json2);
            ViewBag.Account = data2;
            return View(data);
        }
        [HttpPost]
        [Route("/xac-thuc-tai-khoan", Name = "SendTokenVerifyEmail")]
        public async Task<IActionResult> SendTokenVerifyEmail()
        {
            var response = await client.PostAsync($"api/auth/token-verify-email", null);
            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            return Json(new { Status = true });
        }
        [HttpPost]
        [Route("/xac-thuc-tai-khoan-doi-tac", Name = "SendTokenVerifyEmail2")]
        public async Task<IActionResult> SendTokenVerifyEmail2()
        {
            var response = await client.PostAsync($"api/auth/token-verify-email-partner", null);
            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            return Json(new { Status = true });
        }

        [HttpGet]
        [Route("/verify-email", Name = "VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var returnUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            if (User.Identity.Name == null) return Redirect($"/dang-nhap?returnUrl={returnUrl}");
            var json = JsonConvert.SerializeObject(new { token = token });
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auth/verify-email", stringContent);
            var accountType = User.Claims.FirstOrDefault(x => x.Type.Equals("AccountType")).Value;

            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (accountType == "2") return Redirect("/thong-tin-ca-nhan");

                return Redirect("/thong-tin-doi-tac");
            }
            if (accountType == "2") return Redirect("/thong-tin-ca-nhan");

            return Redirect("/thong-tin-doi-tac");

        }

        [Route("/dang-nhap", Name = "account-sign-in")]
        public IActionResult SignInAsync(string returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);

        }


        [HttpPost]
        [Route("/dang-nhap", Name = "account-sign-in")]
        public async Task<IActionResult> SignInAsync(LoginViewModel request)
        {
            int CUSTOMER = 2;
            request.AccountTypeId = CUSTOMER;
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập Tài khoản/ Email/ Số điện thoại";
                return View();
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập mật khẩu";
                return View();
            }
            var item = new AuthenticateRequestWeb
            {
                AccountTypeId = request.AccountTypeId,
                Password = request.Password,
                Username = request.Username
            };
           
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auth/authenticateweb", stringContent);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string jsonResult = response.Content.ReadAsStringAsync().Result;
                dynamic dataResult = JsonConvert.DeserializeObject<dynamic>(jsonResult);
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = dataResult.message;
                return View();
            }

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            AuthenticateResponse data = JsonConvert.DeserializeObject<AuthenticateResponse>(jsonResponse);

            // create claims
            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, data.Id),
                    new Claim(ClaimTypes.NameIdentifier, data.FullName.ToSafetyString()),
                    new Claim(ClaimTypes.MobilePhone, data.Phone.ToSafetyString()),
                    new Claim("Avatar", data.Avatar.ToSafetyString()),
                    new Claim("JwtToken", data.JwtToken.ToSafetyString()),
                    new Claim("AccountType", request.AccountTypeId.ToSafetyString()),
                    new Claim("IsVerified", data.IsVerified.ToSafetyString()),
                    new Claim(ClaimTypes.Email, data.Email.ToSafetyString())
                };
            // create identity
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            // sign-in
            await HttpContext.SignInAsync(
                    scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                    principal: principal,
                    properties: new AuthenticationProperties
                    {
                        IsPersistent = request.RememberMe, // for 'remember me' feature
                                                           //ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
                    });
            if (request.ReturnUrl.IsNullOrEmpty())
                return RedirectToAction("Index", "Home");
            return Redirect(request.ReturnUrl);
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

        [Route("/dang-nhap-facebook", Name = "account-sign-in-facebook")]
        public async Task<IActionResult> SignInFacebookAsync()
        {
            //await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme);
            return new ChallengeResult(FacebookDefaults.AuthenticationScheme);
        }
      
        [HttpPost]
        [Route("/dang-ky", Name = "sign-up")]
        public async Task<IActionResult> SignUpAsync(RegisterCustomerViewModel request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập họ và tên";
                return View();
            }

            if (string.IsNullOrWhiteSpace(request.Email) && !IsValidEmail(request.Email))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập email và phải đúng định dạng email";
                return View();
            }

            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập số điện thoại";
                return View();
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập mật khẩu";
                return View();
            }
            var requestRegister = new RegisterCustomerRequest(
                request.Username,
                request.Password,
                request.Phone, null,
                request.ReferralCode,
                request.Email,
                request.FullName);


            var json = JsonConvert.SerializeObject(requestRegister);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auth/registercustomer", stringContent);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            ProcessingResult data = JsonConvert.DeserializeObject<ProcessingResult>(jsonResponse);
            if (data.MessageType == MessageTypeEnum.Danger)
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = data.Message;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [Route("/dang-ky", Name = "sign-up")]
        public IActionResult SignUpAsync()
        {
            return View();
        }
        [HttpPost]
        [Route("/dang-ky-tai-khoan-doi-tac", Name = "sign-up-partner")]
        public async Task<IActionResult> SignUpPartnerAsync(RegisterPartnerViewModel request)
        {

            var partnerTypeResponse = await client.GetAsync($"api/partnertype/getallstatus?status=true");

            if (partnerTypeResponse == null)
                return View();

            if (partnerTypeResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string partnerTypeJson = partnerTypeResponse.Content.ReadAsStringAsync().Result;
            List<PartnerTypeModel> partnerTypeData = JsonConvert.DeserializeObject<List<PartnerTypeModel>>(partnerTypeJson);
            ViewBag.PartnerType = partnerTypeData;
            if (string.IsNullOrWhiteSpace(request.Representative))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập người đại diện";
                return View();
            }

            if (string.IsNullOrWhiteSpace(request.Email) && !IsValidEmail(request.Email))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập email và phải đúng định dạng email";
                return View();
            }

            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập số điện thoại";
                return View();
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập mật khẩu";
                return View();
            }
            var requestRegister = new RegisterPartnerRequest(
                request.Username,
                request.Password,
                request.Phone, null,
                request.Title,
                request.Email,
                request.Representative);


            var json = JsonConvert.SerializeObject(requestRegister);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auth/registerpartner", stringContent);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            ProcessingResult data = JsonConvert.DeserializeObject<ProcessingResult>(jsonResponse);
            if (data.MessageType == MessageTypeEnum.Danger)
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = data.Message;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [Route("/dang-ky-tai-khoan-doi-tac", Name = "sign-up-partner")]
        public async Task<IActionResult> SignUpPartnerAsync()
        {
            var partnerTypeResponse = await client.GetAsync($"api/partnertype/getallstatus?status=true");

            if (partnerTypeResponse == null)
                return View();

            if (partnerTypeResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string partnerTypeJson = partnerTypeResponse.Content.ReadAsStringAsync().Result;
            List<PartnerTypeModel> partnerTypeData = JsonConvert.DeserializeObject<List<PartnerTypeModel>>(partnerTypeJson);
            ViewBag.PartnerType = partnerTypeData;
            return View();
        }
        [Route("/dang-xuat", Name = "SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [Route("/quen-mat-khau", Name = "forgot-password")]
        public IActionResult ForGotPassword()
        {
            return View();
        }
        [HttpPost]

        [Route("/quen-mat-khau", Name = "forgot-password")]
        public async Task<IActionResult> ForGotPasswordAsync(ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) && !IsValidEmail(request.Email))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập email và phải đúng định dạng email";
                return View();
            }
            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/auth/forgot-password", stringContent);
            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string jsonResult = response.Content.ReadAsStringAsync().Result;
                dynamic dataResult = JsonConvert.DeserializeObject<dynamic>(jsonResult);
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = dataResult.message;
                return View();
            }
            return Redirect("dang-nhap");
        }
        [HttpPost]
        [Route("/doi-mat-khau", Name = "change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OldPassword))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập mật khẩu cũ";
                return View();
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập mật khẩu mới";
                return View();
            }
            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/account/change-password", stringContent);
            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string jsonResult = response.Content.ReadAsStringAsync().Result;
                dynamic dataResult = JsonConvert.DeserializeObject<dynamic>(jsonResult);
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = dataResult.message;
                return View();
            }
            return Redirect("trang-chu");

        }
        [Route("/doi-mat-khau", Name = "change-password")]
        public IActionResult ChangePassword()
        {

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody]ChangePasswordRequest request)
        {
           
            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/account/change-password", stringContent);
            if (response == null)
                return Ok(new {
                    status = false
                });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                    return Ok(new {
                    status = false
                });
            }
            return Ok(new {
                status = true
            });
        }

      
     
      
        [Route("/danh-sach-don-hang", Name = "order-list")]
        public IActionResult OrderList()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInfo([FromBody]UpdateInfoRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/customer/UpdateInfo", stringContent);

            if (response == null)
                return Ok(new {
                    status = false
                });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Ok(new {
                    status = false
                });
            }
            return Ok(new {
                status = true
            });
        }
    }
}
