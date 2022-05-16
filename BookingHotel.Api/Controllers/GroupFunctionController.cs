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
    [Route("api/groupfunction")]
    [ApiController]
    public class GroupFunctionController : ControllerBase
    {
        #region Fields
        private readonly IGroupFunctionService _groupfunctionService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public GroupFunctionController(IGroupFunctionService groupfunctionService, ILogging logging)
        {
            _groupfunctionService = groupfunctionService;
            _logging = logging;
        }
        #endregion


        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _groupfunctionService.FindById(id));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _groupfunctionService.GetByStatusAsync(status));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _groupfunctionService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _groupfunctionService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] GroupFunctionModel model)
        {
            return Ok(await _groupfunctionService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] GroupFunctionModel model)
        {
            return Ok(await _groupfunctionService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _groupfunctionService.DeleteAsync(id));
        }

        [HttpPost("PostFunctionsGroupFunction")]
        public async Task<IActionResult> PostFunctionsGroupFunction([FromBody] PostGroupFunctionRequest request)
        {
            return Ok(await _groupfunctionService.PostFunctionGroupFunction(request));
        }
        [HttpPut("PutFunctionsGroupFunction")]
        public async Task<IActionResult> PutFunctionsGroupFunction([FromBody] PutGroupFunctionRequest request)
        {
            return Ok(await _groupfunctionService.PutFunctionGroupFunction(request));
        }
        [HttpGet("GetFunctionsGroupFunction/{groupFunctionId}")]
        public async Task<IActionResult> GetFunctionsGroupFunction(int groupFunctionId)
        {
            return Ok(await _groupfunctionService.GetFunctionsGroupFunction(groupFunctionId));
        }
    }
}
