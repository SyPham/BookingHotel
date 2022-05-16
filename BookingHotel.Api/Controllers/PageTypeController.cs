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
    [Route("api/pagetype")]
    [ApiController]
    public class PageTypeController : ControllerBase
    {
        #region Fields
        private readonly IPageTypeService _pagetypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public PageTypeController(IPageTypeService pagetypeService, ILogging logging)
        {
            _pagetypeService = pagetypeService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _pagetypeService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _pagetypeService.PaginationAsync(paramater));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pagetypeService.GetAllAsync());
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PageTypeModel model)
        {

            return Ok(await _pagetypeService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PageTypeModel model)
        {

            return Ok(await _pagetypeService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _pagetypeService.DeleteAsync(id));
        }
    }
}