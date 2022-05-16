using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Implementation.Mobile;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common.Helpers;

namespace BookingHotel.Api.ControllerMobile
{

    public class ManagerMobileController : BaseController
    {
        private readonly IMobileBaseService _service;

        public ManagerMobileController(
            IMobileBaseService service)
        {
            _service = service;
        }
        //Danh sach san pham
        [HttpGet]
        public async Task<IActionResult> GetAllProductByPartnerId()
        {
            var accessToken = Request.Headers["Authorization"];
            int partnerId = JWTExtensions.GetDecodeTokenByPartnerId(accessToken);
            //
            return Ok(await _service.GetAllProductByPartnerId( partnerId));
        }
        // chi tiet san pham
        [HttpGet]
        public async Task<IActionResult> GetByProductId([FromQuery] int productId)
        {
            return Ok(await _service.GetByProductId(productId));
        }
        //Xem nguoi quan tam
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerByPartnerId()
        {
            var accessToken = Request.Headers["Authorization"];
            int partnerId = JWTExtensions.GetDecodeTokenByPartnerId(accessToken);
            //
            return Ok(await _service.GetAllCustomerByPartnerId(partnerId));
        }
        //Xem Danh gia
        [HttpGet]
        public async Task<IActionResult> GetAllFeedback()
        {
            return Ok(await _service.GetAllFeedback());
        }

        //Xem 10 san pham co luot view cao
        [HttpGet]
        public async Task<IActionResult> GetViewTimeProductTop10ByPartnerId()
        {
            var accessToken = Request.Headers["Authorization"];
            int partnerId = JWTExtensions.GetDecodeTokenByPartnerId(accessToken);
            //
            return Ok(await _service.GetViewTimeProductTop10ByPartnerId(partnerId));
        }

        //Xem 10 san pham co luot sold cao
        [HttpGet]
        public async Task<IActionResult> GetSoldViewProductTop10ByPartnerId()
        {
            var accessToken = Request.Headers["Authorization"];
            int partnerId = JWTExtensions.GetDecodeTokenByPartnerId(accessToken);
            //
            return Ok(await _service.GetSoldViewProductTop10ByPartnerId(partnerId));
        }
    }
}
