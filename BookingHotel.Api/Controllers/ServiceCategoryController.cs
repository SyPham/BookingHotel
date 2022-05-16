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
    [Route("api/servicecategory")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        #region Fields
        private readonly IServiceCategoryService _serviceCatService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ServiceCategoryController(IServiceCategoryService serviceCatService, ILogging logging)
        {
            _serviceCatService = serviceCatService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _serviceCatService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _serviceCatService.GetByStatusAsync(status));
        }

        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int parentId)
        {
            return Ok(await _serviceCatService.GetAllGroupAsync(parentId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _serviceCatService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _serviceCatService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ServiceCategoryModel model)
        {

            return Ok(await _serviceCatService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ServiceCategoryModel model)
        {

            return Ok(await _serviceCatService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _serviceCatService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _serviceCatService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _serviceCatService.DeleteRangeAsync(id));
        }
    }
}
