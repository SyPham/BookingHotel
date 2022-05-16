using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/notificationaccount")]
    [ApiController]
    public class NotificationAccountController : ControllerBase
    {
        #region Fields
        private readonly INotificationAccountService _notificationAccountService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public NotificationAccountController(INotificationAccountService notificationAccountService, ILogging logging)
        {
            _notificationAccountService = notificationAccountService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _notificationAccountService.GetAllAsync());
        }
        [HttpGet("getnotificationsbyaccount")]
        public async Task<IActionResult> GetNotificationByAccount(bool status)
        {
            return Ok(await _notificationAccountService.GetNotificationByAccount(status));
        }
        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _notificationAccountService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _notificationAccountService.FindById(id));
        }

        [HttpGet("getbymultiid")]
        public async Task<IActionResult> GetByID([FromQuery] int accountId, [FromQuery] int notificationId)
        {
            return Ok(await _notificationAccountService.FindById(accountId, notificationId));
        }

        [Authorize]
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] NotificationAccountModel model)
        {
            return Ok(await _notificationAccountService.AddAsync(model));
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] NotificationAccountModel model)
        {
            return Ok(await _notificationAccountService.UpdateAsync(model));
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int accountId, [FromQuery] int notificationId)
        {
            return Ok(await _notificationAccountService.DeleteAsync(accountId, notificationId));
        }
    }
}
