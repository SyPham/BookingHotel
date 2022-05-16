using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/partnertype")]
    [ApiController]
    public class PartnerTypeController : ControllerBase
    {
        #region Fields
        private readonly IPartnerTypeService _partnerTypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public PartnerTypeController(IPartnerTypeService partnerTypeService, ILogging logging)
        {
            _partnerTypeService = partnerTypeService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _partnerTypeService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _partnerTypeService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _partnerTypeService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _partnerTypeService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PartnerTypeModel model)
        {
            
            return Ok(await _partnerTypeService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PartnerTypeModel model)
        {
           
            return Ok(await _partnerTypeService.UpdateAsync(model));
        }

        [HttpPost("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _partnerTypeService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _partnerTypeService.DeleteAsync(id));
        }
    }
}
