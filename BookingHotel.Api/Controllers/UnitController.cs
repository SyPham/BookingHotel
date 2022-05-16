using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/unit")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        #region Fields
        private readonly IUnitService _unitService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public UnitController(IUnitService unitService, ILogging logging)
        {
            _unitService = unitService;
            _logging = logging;
        }
        #endregion


        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _unitService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _unitService.FindById(id));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _unitService.GetByStatusAsync(status));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] UnitModel model)
        {
            return Ok(await _unitService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UnitModel model)
        {
            return Ok(await _unitService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _unitService.DeleteAsync(id));
        }


    }
}
