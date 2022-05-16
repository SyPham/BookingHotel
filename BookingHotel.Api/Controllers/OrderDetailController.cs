using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/orderdetail")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        #region Fields
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public OrderDetailController(IOrderDetailService orderDetailService, ILogging logging)
        {
            _orderDetailService = orderDetailService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderDetailService.GetAllAsync());
        }

        [HttpGet("getbyorder")]
        public async Task<IActionResult> GetByOrder([FromQuery] int orderId)
        {
            return Ok(await _orderDetailService.GetByOrderAsync(orderId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _orderDetailService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _orderDetailService.FindById(id));
        }

        [HttpGet("getbymultiid")]
        public async Task<IActionResult> GetByID([FromQuery] int productId, [FromQuery] int tabId)
        {
            return Ok(await _orderDetailService.FindById(productId, tabId));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] OrderDetailModel model)
        {

            return Ok(await _orderDetailService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] OrderDetailModel model)
        {

            return Ok(await _orderDetailService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _orderDetailService.DeleteAsync(id));
        }

        [HttpDelete("deletebymultiid")]
        public async Task<IActionResult> DeleteByKey([FromQuery] int orderId, [FromQuery] int productId)
        {
            return Ok(await _orderDetailService.DeleteByKeyAsync(orderId, productId));
        }

        [HttpDelete("deletebyorder")]
        public async Task<IActionResult> DeleteByOrder([FromQuery] int orderId)
        {
            return Ok(await _orderDetailService.DeleteByOrderAsync(orderId));
        }
    }
}
