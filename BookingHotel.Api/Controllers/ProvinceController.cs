using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/province")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        #region Fields
        private readonly IProvinceService _provinceService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ProvinceController(IProvinceService provinceService, ILogging logging)
        {
            _provinceService = provinceService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _provinceService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _provinceService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _provinceService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _provinceService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ProvinceModel model)
        {
            
            return Ok(await _provinceService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ProvinceModel model)
        {
           
            return Ok(await _provinceService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _provinceService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _provinceService.DeleteAsync(id));
        }
    }
}
