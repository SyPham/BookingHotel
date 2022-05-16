using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/orderstatus")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        #region Fields
        private readonly IOrderStatusService _orderStatusService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public OrderStatusController(IOrderStatusService orderstatusService, ILogging logging)
        {
            _orderStatusService = orderstatusService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderStatusService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _orderStatusService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _orderStatusService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _orderStatusService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] OrderStatusModel model)
        {
            
            return Ok(await _orderStatusService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] OrderStatusModel model)
        {
           
            return Ok(await _orderStatusService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _orderStatusService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _orderStatusService.DeleteAsync(id));
        }
    }
}
