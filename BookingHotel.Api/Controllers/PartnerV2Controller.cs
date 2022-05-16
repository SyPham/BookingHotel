using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;
using Microsoft.AspNetCore.Authorization;

namespace BookingHotel.Api.Controllers
{
    [Route("api/partnerv2")]
    [ApiController]
    public class PartnerV2Controller : ControllerBase
    {
        #region Fields
        private readonly IPartnerService _partnerService;
        private readonly IAccountService _accountService;
        private readonly ILogging _logging;
        private readonly IWebHostEnvironment _currentEnvironment;
        #endregion

        #region Ctor
        public PartnerV2Controller(IPartnerService partnerService, IAccountService accountService, ILogging logging, IWebHostEnvironment currentEnvironment)
        {
            _partnerService = partnerService;
            _accountService = accountService;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _partnerService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _partnerService.GetByStatusAsync(status));
        }
        [HttpGet("getcustomersbypartnerid")]
        public async Task<IActionResult> GetCustomersByPartnerId([FromQuery] int partnerId)
        {
            return Ok(await _partnerService.GetCustomersByPartnerId(partnerId));
        }
        
        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int typeId)
        {
            return Ok(await _partnerService.GetAllGroupAsync(typeId));
        }

        [HttpGet("getallbyaccount")]
        public async Task<IActionResult> GetAllByAccount([FromQuery] int accountId)
        {
            return Ok(await _partnerService.GetAllByAccountAsync(accountId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _partnerService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _partnerService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] PartnerModel model)
        {
            return Ok(await _partnerService.AddWithUploadAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] PartnerModel model)
        {
            return Ok(await _partnerService.UpdateWithUploadAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _partnerService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _partnerService.DeleteWithUploadAsync(id));

        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> ids)
        {
            return Ok(await _partnerService.DeleteRangeWithUploadAsync(ids));

        }
        [Authorize]
        [HttpPut("UploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request)
        {
            return Ok(await _partnerService.UploadAvatar(request));
        }
    }
}
