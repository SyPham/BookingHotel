using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/partnerlocal")]
    [ApiController]
    public class PartnerLocalController : ControllerBase
    {
        #region Fields
        private readonly IPartnerLocalService _partnerLocalService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public PartnerLocalController(IPartnerLocalService partnerLocalService, ILogging logging)
        {
            _partnerLocalService = partnerLocalService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _partnerLocalService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _partnerLocalService.GetByStatusAsync(status));
        }

        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int partnerId)
        {
            return Ok(await _partnerLocalService.GetAllGroupAsync(partnerId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _partnerLocalService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _partnerLocalService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PartnerLocalModel model)
        {
            
            return Ok(await _partnerLocalService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PartnerLocalModel model)
        {
           
            return Ok(await _partnerLocalService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _partnerLocalService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _partnerLocalService.DeleteAsync(id));
        }
    }
}
