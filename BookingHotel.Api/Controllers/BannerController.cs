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
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/banner")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        #region Fields
        private readonly IBannerService _bannerService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public BannerController(IBannerService bannerService, ILogging logging)
        {
            _bannerService = bannerService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bannerService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _bannerService.GetByStatusAsync(status));
        }
[HttpGet("getrandom")]
        public async Task<IActionResult> GetRandom([FromQuery] int NumberItem)
        {
            return Ok(await _bannerService.GetAllRandomAsync(NumberItem));
        }
        [HttpGet("gettop")]
        public async Task<IActionResult> GetTopNumberAsync([FromQuery] int NumberItem)
        {
            return Ok(await _bannerService.GetTopNumberAsync(NumberItem));
        }
        [HttpGet("GetMainBanner")]
        public async Task<IActionResult> GetMainBanner([FromQuery] int NumberItem)
        {
            return Ok(await _bannerService.GetMainBanner(NumberItem));
        }
        [HttpGet("GetCenterBanner")]
        public async Task<IActionResult> GetCenterBanner()
        {
            return Ok(await _bannerService.GetCenterBanner());
        }
        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _bannerService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _bannerService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] BannerModel model)
        {
            return Ok(await _bannerService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] BannerModel model)
        {
            return Ok(await _bannerService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _bannerService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _bannerService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _bannerService.DeleteRangeAsync(id));
        }
    }
}
