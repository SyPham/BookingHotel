using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/methodused")]
    [ApiController]
    public class MethodUsedController : ControllerBase
    {
        #region Fields
        private readonly IMethodUsedService _methodUsedService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public MethodUsedController(IMethodUsedService methodUsedService, ILogging logging)
        {
            _methodUsedService = methodUsedService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _methodUsedService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _methodUsedService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _methodUsedService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _methodUsedService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] MethodUsedModel model)
        {
            return Ok(await _methodUsedService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] MethodUsedModel model)
        {

            return Ok(await _methodUsedService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _methodUsedService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _methodUsedService.DeleteAsync(id));
        }
    }
}
