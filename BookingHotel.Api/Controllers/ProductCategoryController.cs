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
    [Route("api/productcategory")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        #region Fields
        private readonly IProductCategoryService _productCatService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ProductCategoryController(IProductCategoryService productCatService, ILogging logging)
        {
            _productCatService = productCatService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productCatService.GetAllAsync());
        }
        [HttpGet("getmenus")]
        public async Task<IActionResult> GetMenus()
        {
            return Ok(await _productCatService.GetMenus());
        }
        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int parentId)
        {
            return Ok(await _productCatService.GetAllGroupAsync(parentId));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _productCatService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _productCatService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _productCatService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] ProductCategoryModel model)
        {

            return Ok(await _productCatService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ProductCategoryModel model)
        {

            return Ok(await _productCatService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _productCatService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _productCatService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _productCatService.DeleteRangeAsync(id));
        }

        [HttpGet("gettop")]
        public async Task<IActionResult> GetTop([FromQuery] int NumberItem)
        {
            return Ok(await _productCatService.GetAllTopAsync(NumberItem));
        }
        [HttpGet("GetBreadcrumbById")]
        public async Task<IActionResult> GetBreadcrumbById([FromQuery] int id)
        {
            return Ok(await _productCatService.GetBreadcrumbById(id));
        }
    }
}
