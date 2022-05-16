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
    [Route("api/page")]
    [ApiController]
    public class PageController : ControllerBase
    {
        #region Fields
        private readonly IPageService _pageService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public PageController(IPageService pageService, ILogging logging)
        {
            _pageService = pageService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _pageService.FindById(id));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _pageService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _pageService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PageModel model)
        {
            return Ok(await _pageService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PageModel model)
        {
            return Ok(await _pageService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _pageService.DeleteAsync(id));
        }
    }
}
