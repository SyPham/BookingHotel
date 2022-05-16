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
    [Route("api/customertype")]
    [ApiController]
    public class CustomerTypeController : ControllerBase
    {
        #region Fields
        private readonly ICustomerTypeService _customerTypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public CustomerTypeController(ICustomerTypeService customerTypeService, ILogging logging)
        {
            _customerTypeService = customerTypeService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _customerTypeService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _customerTypeService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _customerTypeService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] CustomerTypeModel model)
        {
            return Ok(await _customerTypeService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] CustomerTypeModel model)
        {
            return Ok(await _customerTypeService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _customerTypeService.DeleteAsync(id));
        }
    }
}
