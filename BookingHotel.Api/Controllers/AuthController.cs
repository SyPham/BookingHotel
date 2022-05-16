using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        private readonly ICustomerService _customerService;
        private readonly IPartnerService _partnerService;
        private readonly IOptions<AppSettings> _config;

        public AuthController(
            IAccountService accountService, 
            IAuthService authService, 
            ICustomerService customerService,
            IPartnerService partnerService,
            IOptions<AppSettings> config
            )
        {
            _accountService = accountService;
            _authService = authService;
            _customerService = customerService;
            _partnerService = partnerService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model, ipAddress(), 2); // là tk khách hàng

            if (response == null)
                return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("authenticateweb")]
        public IActionResult AuthenticateWeb([FromBody] AuthenticateRequestWeb model)
        {
            var response = _accountService.AuthenticateWeb(model, ipAddress()); 

            if (response == null)
                return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenticateRequest model)
        {
            var response = await _authService.Authenticate(model, ipAddress()); // là tk admin

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.Data.RefreshToken);

            return Ok(response.Data);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return Ok(await _authService.Register(request));
        }
        [AllowAnonymous]
        [HttpPost("refresh-tokenv2")]
        public async Task<IActionResult> RefreshTokenV2Async()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _authService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("registerpartner")]
        public async Task<IActionResult> RegisterPartner([FromBody] RegisterPartnerRequest request)
        {
            return Ok(await _authService.RegisterPartner(request));
        }
        [AllowAnonymous]
        [HttpPost("registercustomer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request)
        {
            return Ok(await _authService.RegisterCustomer(request));
        }

        [Authorize]
        [HttpPost("revoke-tokenv2")]
        public async Task<IActionResult> RevokeTokenV2Async([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            //if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
            //    return Unauthorized(new { message = "Unauthorized" });

            var response = await _authService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _accountService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            //if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
            //    return Unauthorized(new { message = "Unauthorized" });

            var response = _accountService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
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
            string origin = _config.Value.WebOrigin;
            return Ok(await _authService.SendTokenVerifyEmail(accountId, customer.Email, origin));
        }
        [Authorize]
        [HttpPost("token-verify-email-partner")]
        public async Task<IActionResult> SendTokenVerifyEmailPartner()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            var partner = _partnerService.GetByAccount(accountId);
            if (partner == null) return NotFound(new { message = "Thông tin không tồn tại" });
            //
            string origin = _config.Value.WebOrigin;
            return Ok(await _authService.SendTokenVerifyEmail(accountId, partner.Email, origin));
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            return Ok(await _accountService.VerifyEmail(model.Token));
        }
    }
}
