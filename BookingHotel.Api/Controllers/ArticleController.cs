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
    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        #region Fields
        private readonly IArticleService _articleService;
        private readonly ILogging _logging;
        private readonly IHostingEnvironment _currentEnvironment;
        #endregion

        #region Ctor
        public ArticleController(IArticleService articleService, ILogging logging, IHostingEnvironment currentEnvironment)
        {
            _articleService = articleService;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _articleService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _articleService.GetByStatusAsync(status));
        }

        [HttpGet("getallbycategory")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] int catId)
        {
            return Ok(await _articleService.GetAllByCategoryAsync(catId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _articleService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _articleService.FindById(id));
        }

        [HttpGet("gethot")]
        public async Task<IActionResult> GetHot([FromQuery] int NumberItem)
        {
            return Ok(await _articleService.GetAllHotAsync(NumberItem));
        }

        [HttpGet("getnew")]
        public async Task<IActionResult> GetNew([FromQuery] int NumberItem)
        {
            return Ok(await _articleService.GetAllNewAsync(NumberItem));
        }

        [HttpGet("getrandom")]
        public async Task<IActionResult> GetRandom([FromQuery] int NumberItem)
        {
            return Ok(await _articleService.GetAllRandomAsync(NumberItem));
        }

        [HttpGet("gettop")]
        public async Task<IActionResult> GetTop([FromQuery] int NumberItem)
        {
            return Ok(await _articleService.GetAllTopAsync(NumberItem));
        }

        [HttpGet("getrelated")]
        public async Task<IActionResult> GetRelated([FromQuery] int id, [FromQuery] int catId, [FromQuery] int NumberItem)
        {
            return Ok(await _articleService.GetAllRelatedAsync(id, catId, NumberItem));
        }

        [HttpPut("updateview")]
        public async Task<IActionResult> UpdateView([FromQuery] int id)
        {
            return Ok(await _articleService.UpdateViewAsync(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] ArticleModel model)
        {
            return Ok(await _articleService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ArticleModel model)
        {
            return Ok(await _articleService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _articleService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _articleService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _articleService.DeleteRangeAsync(id));
        }
    }
}
