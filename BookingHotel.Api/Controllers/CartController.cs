using AutoMapper;
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
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICartLiteService _cartLiteService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public CartController(ICartService cartService, ICartLiteService cartLiteService, ILogging logging)
        {
            _cartService = cartService;
            _cartLiteService = cartLiteService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.GetByAccountAsync(accountId));
        }

        [HttpGet("getbyproduct")]
        public async Task<IActionResult> GetByProduct([FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.FindById(accountId, productId));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] CartLiteModel model)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartLiteService.AddAsync(model, accountId));
        }

        //[Authorize]
        //[HttpPut("update")]
        //public async Task<IActionResult> Update([FromBody] CartLiteModel model)
        //{
        //    var accessToken = Request.Headers["Authorization"];
        //    int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

        //    return Ok(await _cartLiteService.UpdateAsync(model, accountId));
        //}


        [HttpPut("updatequantity")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int productId, [FromQuery] int qty)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.UpdateQuantityAsync(accountId, productId, qty));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.DeleteAsync(accountId, productId));
        }

        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAll()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.DeleteByAccountAsync(accountId));
        }
        [HttpDelete("clearcart")]
        public async Task<IActionResult> ClearCart()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.ClearCart(accountId));
        }
        [HttpDelete("deletecartitem")]
        public async Task<IActionResult> DeleteCartItem(int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.DeleteCartItem(accountId,productId));
        }
        [HttpPost("additemintocart")]
        public async Task<IActionResult> AddItemintoCart([FromBody] CartModel cartModel)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.AddItemintoCart(cartModel));
        }
        [HttpPut("changecartitemquantity")]
        public async Task<IActionResult> ChangeCartItemQuantity(int productId)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.ChangeCartItemQuantity(accountId, productId));
        }
           [HttpPut("updatecartitemquantity")]
        public async Task<IActionResult> UpdateCartItemQuantity(int productId, int quantity)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.UpdateCartItemQuantity(accountId, productId, quantity));
        }
        [HttpGet("getcartitems")]
        public async Task<IActionResult> GetCartItems()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _cartService.GetCartItems(accountId));
        }
    }
}
