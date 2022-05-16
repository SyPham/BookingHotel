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
    [Route("api/module")]
    [ApiController]
    public class ModuleController : ControllerBase
    {

        #region Fields
        private readonly IModuleService _moduleService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ModuleController(IModuleService moduleService, ILogging logging)
        {
            _moduleService = moduleService;
            _logging = logging;
        }
        #endregion


        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _moduleService.FindById(id));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetByID()
        {
            return Ok(await _moduleService.GetAllAsync());
        }
        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _moduleService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _moduleService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ModuleModel model)
        {
            return Ok(await _moduleService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ModuleModel model)
        {
            return Ok(await _moduleService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _moduleService.DeleteAsync(id));
        }
    }
}
