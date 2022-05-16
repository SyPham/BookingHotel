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
    [Route("api/paytype")]
    [ApiController]
    public class PayTypeController : ControllerBase
    {
        #region Fields
        private readonly IPayTypeService _paytypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public PayTypeController(IPayTypeService payTypeService, ILogging logging)
        {
            _paytypeService = payTypeService;
            _logging = logging;
        }
        #endregion


        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _paytypeService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _paytypeService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _paytypeService.FindById(id));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _paytypeService.GetByStatusAsync(status));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PayTypeModel model)
        {
            return Ok(await _paytypeService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PayTypeModel model)
        {
            return Ok(await _paytypeService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _paytypeService.DeleteAsync(id));
        }

    }
}
