using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;
using System.Collections.Generic;

namespace BookingHotel.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        #region Fields
        ProcessingResult processingResult;
        private readonly IOrderService _orderService;
        private readonly IOrderLiteService _orderLiteService;
        private readonly ICustomerService _customerService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public OrderController(IOrderService orderService, IOrderLiteService orderLiteService, ICustomerService customerService, ILogging logging)
        {
            _orderService = orderService;
            _orderLiteService = orderLiteService;
            _customerService = customerService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] int status)
        {
            return Ok(await _orderService.GetByStatusAsync(status));
        }

        [HttpGet("getbyorderstatus")]
        public async Task<IActionResult> GetByOrderStatus([FromQuery] int orderStatusId)
        {
            return Ok(await _orderService.GetByOrderStatusAsync(orderStatusId));
        }

        [HttpGet("getbypaytype")]
        public async Task<IActionResult> GetByPayType([FromQuery] int payTypeId)
        {
            return Ok(await _orderService.GetByPayTypeAsync(payTypeId));
        }

        [HttpGet("getbycustomer")]
        public async Task<IActionResult> GetByCustomer()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            CustomerModel customer = await _customerService.GetByAccountAsync(accountId);
            //
            if (customer != null)
                return Ok(await _orderService.GetByCustomerAsync(customer.Id));
            return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false });
        }

        [HttpGet("getbycustomerpagination")]
        public async Task<IActionResult> GetByCustomerPagination([FromQuery] Filter2Pagination paramater)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            CustomerModel customer = await _customerService.GetByAccountAsync(accountId);
            //
            if (customer != null)
                return Ok(await _orderService.GetByCustomerPaginationAsync(paramater, customer.Id));
            return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false });
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _orderService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _orderService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] OrderLiteModel model)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //

            processingResult = await _orderLiteService.AddAsync(model, accountId);
            if (processingResult.Success)
            {
                CustomerModel customer = await _customerService.GetByAccountAsync(accountId);
                //
                await _customerService.UpdatePointAsync(customer.Id, model.Point.ToInt(), 2);
            }
            return Ok(processingResult);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] OrderModel model)
        {
            return Ok(await _orderService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id, [FromQuery] int status)
        {
            return Ok(await _orderService.UpdateStatusAsync(id, status));
        }
        [HttpPut("updateorderstatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] int id, [FromQuery] int orderStatusId)
        {
            return Ok(await _orderService.UpdateOrderStatusAsync(id, orderStatusId));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRangeAsync([FromQuery] List<int> id)
        {
            return Ok(await _orderService.DeleteRangeAsync(id));
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _orderService.DeleteAsync(id));
        }
        [HttpPost("saveorder")]
        public async Task<IActionResult> SaveOrder([FromBody] OrderModel model)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            processingResult = await _orderService.SaveOrder(model);
            if (processingResult.Success)
            {
                CustomerModel customer = await _customerService.GetByAccountAsync(accountId);
                //
                await _customerService.UpdatePointAsync(customer.Id, model.Point.ToInt(), 2);
            }
            return Ok(processingResult);
        }
    }
}
