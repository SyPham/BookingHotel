using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.Api.Controllers
{
    [Route("api/customerv2")]
    [ApiController]
    public class CustomerV2Controller : ControllerBase
    {
        #region Fields
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly ICustomerLiteService _customerLiteService;
        private readonly IAccountLiteService _accountLiteService;
        private readonly ILogging _logging;
        private readonly IWebHostEnvironment _currentEnvironment;
        #endregion

        #region Ctor
        public CustomerV2Controller(ICustomerService customerService, ICustomerLiteService customerLiteService, IAccountService accountService, IAccountLiteService accountLiteService, ILogging logging, IWebHostEnvironment currentEnvironment)
        {
            _customerService = customerService;
            _customerLiteService = customerLiteService;
            _accountService = accountService;
            _accountLiteService = accountLiteService;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _customerService.FindById(id));
        }

        [HttpGet("getorderbbycustomerid")]
        public async Task<IActionResult> GetOrderByCustomerId([FromQuery] int customerId)
        {
            return Ok(await _customerService.GetOrderByCustomerId(customerId));
        }

        [HttpGet("getproductswishlistbyaccountid")]
        public async Task<IActionResult> GetProductsWishlistByAccountId([FromQuery] int accountId)
        {
            return Ok(await _customerService.GetProductsWishlistByAccountId(accountId));
        }

        [HttpGet("getpurchasedproductsbycustomerid")]
        public async Task<IActionResult> GetPurchasedProductsByCustomerId([FromQuery] int customerId)
        {
            return Ok(await _customerService.GetPurchasedProductsByCustomerId(customerId));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _customerService.GetAllAsync());
        }
        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _customerService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] CustomerModel model)
        {
            return Ok(await _customerService.AddWithUploadAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] CustomerModel model)
        {
            return Ok(await _customerService.UpdateWithUploadAsync(model));

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _customerService.DeleteAsync(id));

        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> ids)
        {
            return Ok(await _customerService.DeleteRangeWithUploadAsync(ids));

        }
        [Authorize]
        [HttpPut("UploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request)
        {
            return Ok(await _customerService.UploadAvatar(request));
        }
    }
}
