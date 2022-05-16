using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Enums;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        #region Fields
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public AccountController(IAccountService accountService, ICustomerService customerService, ILogging logging)
        {
            _accountService = accountService;
            _customerService = customerService;
            _logging = logging;
        }
        #endregion

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            return Ok(await _accountService.ChangePassword(request));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            request.Id = accountId;
            //
            return Ok(await _accountService.ChangePassword(request));
        }

        [Authorize]
        [HttpPost("token-verify-email")]
        public async Task<IActionResult> SendTokenVerifyEmail()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            CustomerModel customer = _customerService.GetByAccount(accountId);
            if (customer == null) return NotFound(new { message = "Thông tin không tồn tại" });
            //
            return Ok(await _accountService.SendTokenVerifyEmail(accountId, customer.Email, ""));
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            return Ok(await _accountService.VerifyEmail(model.Token));
        }
       
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            return Ok(await _accountService.ForgotPassword(model, ""));
        }

        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken(ValidateResetTokenRequest model)
        {
            return Ok(await _accountService.ValidateResetToken(model));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await _accountService.ResetPassword(model));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _accountService.GetAllAsync());
        }

        [HttpGet("getAllByAccountTypeID")]
        public async Task<IActionResult> GetAllByAccountTypeID(int accountTypeId)
        {
            return Ok(await _accountService.GetAllByAccountTypeID(accountTypeId));
        }

        [HttpGet("GetMenusByAccountId")]
        public async Task<IActionResult> GetMenusByAccountId(int accountId)
        {
            return Ok(await _accountService.GetMenusByAccountId(accountId));
        }
        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _accountService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _accountService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] AccountModel model)
        {

            return Ok(await _accountService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AccountModel model)
        {

            return Ok(await _accountService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _accountService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _accountService.DeleteRangeAsync(id));
        }
        [HttpPut("lock")]
        public async Task<IActionResult> Lock([FromQuery] int id)
        {
            return Ok(await _accountService.Lock(id));
        }

        // helper methods
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        [HttpPost("PostAccountFunction")]
        public async Task<IActionResult> PostAccountFunction([FromBody] PostAccountFunctionRequest request)
        {
            return Ok(await _accountService.PostAccountFunction(request));
        }
        [HttpPut("PutAccountFunction")]
        public async Task<IActionResult> PutAccountFunction([FromBody] PutAccountFunctionRequest request)
        {
            return Ok(await _accountService.PutAccountFunction(request));
        }
        [HttpGet("GetFunctionsAccount/{accountId}")]
        public async Task<IActionResult> GetFunctionsAccount(int accountId)
        {
            return Ok(await _accountService.GetFunctionsAccount(accountId));
        }
        [HttpGet("GetActionFunctionByAccountId/{accountId}")]
        public async Task<IActionResult> GetActionFunctionByAccountId(int accountId)
        {
            return Ok(await _accountService.GetActionFunctionByAccountId(accountId));
        }
        

    }
}
