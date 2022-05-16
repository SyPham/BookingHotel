using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/notificationtype")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        #region Fields
        private readonly INotificationTypeService _notificationTypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public NotificationTypeController(INotificationTypeService notificationTypeService, ILogging logging)
        {
            _notificationTypeService = notificationTypeService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _notificationTypeService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _notificationTypeService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _notificationTypeService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _notificationTypeService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] NotificationTypeModel model)
        {
            
            return Ok(await _notificationTypeService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] NotificationTypeModel model)
        {
           
            return Ok(await _notificationTypeService.UpdateAsync(model));
        }

        [HttpPost("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _notificationTypeService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _notificationTypeService.DeleteAsync(id));
        }
    }
}
