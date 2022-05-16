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
    [Route("api/function")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        #region Fields
        private readonly IFunctionService _functionService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public FunctionController(IFunctionService functionService, ILogging logging)
        {
            _functionService = functionService;
            _logging = logging;
        }
        #endregion


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _functionService.GetAllAsync());
        }
        [HttpGet("GetAllFunctionAsTree")]
        public async Task<IActionResult> GetAllFunctionAsTree()
        {
            return Ok(await _functionService.GetAllFunctionAsTree());
        }

        [HttpGet("GetMenus")]
        public async Task<IActionResult> GetMenus()
        {
            return Ok(await _functionService.GetMenus());
        }
        [HttpGet("GetAllPagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _functionService.PaginationAsync(paramater));
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            return Ok(await _functionService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] FunctionModel model)
        {
            return Ok(await _functionService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] FunctionModel model)
        {
            return Ok(await _functionService.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _functionService.DeleteAsync(id));
        }
        [HttpGet("GetFunctionByModuleId/{moduleId}")]
        public async Task<IActionResult> GetFunctionByModuleId(int moduleId)
        {
            return Ok(await _functionService.GetFunctionByModuleId(moduleId));
        }
    }
}
