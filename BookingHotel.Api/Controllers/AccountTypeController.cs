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
    [Route("api/accounttype")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        #region Fields
        private readonly IAccountTypeService _accountTypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public AccountTypeController(IAccountTypeService accountTypeService, ILogging logging)
        {
            _accountTypeService = accountTypeService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _accountTypeService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _accountTypeService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _accountTypeService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] AccountTypeModel model)
        {
            return Ok(await _accountTypeService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AccountTypeModel model)
        {
            return Ok(await _accountTypeService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _accountTypeService.DeleteAsync(id));
        }
    }
}
