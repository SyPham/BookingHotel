using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/articlecategory")]
    [ApiController]
    public class ArticleCategoryController : ControllerBase
    {
        #region Fields
        private readonly IArticleCategoryService _articleCatService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ArticleCategoryController(IArticleCategoryService articleCatService, ILogging logging)
        {
            _articleCatService = articleCatService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _articleCatService.GetAllAsync());
        }

        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int parentId)
        {
            return Ok(await _articleCatService.GetAllGroupAsync(parentId));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _articleCatService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _articleCatService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _articleCatService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ArticleCategoryModel model)
        {

            return Ok(await _articleCatService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ArticleCategoryModel model)
        {

            return Ok(await _articleCatService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _articleCatService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _articleCatService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _articleCatService.DeleteRangeAsync(id));
        }
    }
}
