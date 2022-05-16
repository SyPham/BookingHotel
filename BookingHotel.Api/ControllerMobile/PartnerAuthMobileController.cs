using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Implementation;
using BookingHotel.ApplicationService.Implementation.Mobile;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Auth;

namespace BookingHotel.Api.ControllerMobile
{
   
    public class PartnerAuthMobileController : BaseController
    {
        private readonly IMobileBaseService _service;

        public PartnerAuthMobileController(
            IMobileBaseService service
            )
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticatePartnerRequest model)
        {
            var response = await _service.Authenticate(model, ipAddress(), 3); // là tk partner

            if (response == null)
                return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterPartnerRequest request)
        {
            return Ok(await _service.Register(request));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _service.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            //if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
            //    return Unauthorized(new { message = "Unauthorized" });

            var response = await _service.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            return Ok(await _service.ForgotPassword(model, ""));
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await _service.ResetPassword(model));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            return Ok(await _service.ChangePassword(request));
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            return Ok(await _service.VerifyEmail(model.Token));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendTokenVerifyEmail()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            var partner = _service.GetByAccount(accountId);
            if (partner == null) return NotFound(new { message = "Thông tin không tồn tại" });
            //
            return Ok(await _service.SendTokenVerifyEmail(accountId, partner.Email, ""));
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
    }
}
