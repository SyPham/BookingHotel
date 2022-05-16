using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region Fields
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
        private readonly ILogging _logging;
        private readonly IWebHostEnvironment _currentEnvironment;
        #endregion

        #region Ctor
        public NotificationController(INotificationService notificationService, IAccountService accountService, ILogging logging, IWebHostEnvironment currentEnvironment)
        {
            _notificationService = notificationService;
            _accountService = accountService;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _notificationService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _notificationService.GetByStatusAsync(status));
        }

        [HttpGet("getbytypepagination")]
        public async Task<IActionResult> GetByTypePagination([FromQuery] TypePagination paramater)
        {
            return Ok(await _notificationService.GetByTypePaginationAsync(paramater));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _notificationService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _notificationService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] NotificationModel model)
        {

            return Ok(await _notificationService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] NotificationModel model)
        {

            return Ok(await _notificationService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _notificationService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _notificationService.DeleteAsync(id));
        }


    }
}
