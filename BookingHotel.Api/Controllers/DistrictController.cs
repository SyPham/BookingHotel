using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/district")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        #region Fields
        private readonly IDistrictService _districtService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public DistrictController(IDistrictService districtService, ILogging logging)
        {
            _districtService = districtService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _districtService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _districtService.GetByStatusAsync(status));
        }

        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int provinceId)
        {
            return Ok(await _districtService.GetAllGroupAsync(provinceId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _districtService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _districtService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] DistrictModel model)
        {
            
            return Ok(await _districtService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] DistrictModel model)
        {
           
            return Ok(await _districtService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _districtService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _districtService.DeleteAsync(id));
        }
    }
}
