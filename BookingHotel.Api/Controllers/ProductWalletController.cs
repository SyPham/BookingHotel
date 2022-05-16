using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/productwallet")]
    [ApiController]
    [Authorize]
    public class ProductWalletController : ControllerBase
    {
        #region Fields
        private readonly IProductWalletService _productWalletService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ProductWalletController(IProductWalletService productWalletService, ILogging logging)
        {
            _productWalletService = productWalletService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productWalletService.GetAllAsync());
        }

        [Authorize]
        [HttpGet("getbyaccountpagination")]
        public async Task<IActionResult> GetByAccountPagination([FromQuery] ParamaterPagination paramater)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWalletService.GetByAccountPaginationAsync(paramater, accountId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _productWalletService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int orderId, [FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWalletService.FindById(accountId, orderId, productId));
        }

        [HttpPut("updateuse")]
        public async Task<IActionResult> UpdateUse([FromQuery] int orderId, [FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWalletService.UpdateUseAsync(accountId, orderId, productId));
        }

        [HttpPut("updateassess")]
        public async Task<IActionResult> UpdateAssess([FromQuery] int orderId, [FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWalletService.UpdateAssessAsync(accountId, orderId, productId));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int orderId, [FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWalletService.DeleteAsync(accountId, orderId, productId));
        }
    }
}
