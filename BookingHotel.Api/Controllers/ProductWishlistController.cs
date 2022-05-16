using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.Api.Controllers
{
    [Route("api/productwishlist")]
    [ApiController]
    [Authorize]
    public class ProductWishlistController : ControllerBase
    {
        #region Fields
        private readonly IProductWishlistService _productWishlistService;
        private readonly IProductWishlistLiteService _productWishlistLiteService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ProductWishlistController(IProductWishlistService productWishlistService, IProductWishlistLiteService productWishlistLiteService, ILogging logging)
        {
            _productWishlistService = productWishlistService;
            _productWishlistLiteService = productWishlistLiteService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productWishlistService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _productWishlistService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ProductWishlistLiteModel model)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWishlistLiteService.AddAsync(model, accountId));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWishlistService.DeleteAsync(accountId, productId));
        }

        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAll()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productWishlistService.DeleteByAccountAsync(accountId));
        }
    }
}
