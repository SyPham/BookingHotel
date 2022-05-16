using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;
using System.Collections.Generic;

namespace BookingHotel.Api.Controllers
{
    [Route("api/sale")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        #region Fields
        private readonly ISaleService _saleService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public SaleController(ISaleService saleService, ILogging logging)
        {
            _saleService = saleService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _saleService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _saleService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _saleService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _saleService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] SaleModel model)
        {

            return Ok(await _saleService.AddWithUploadAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] SaleModel model)
        {

            return Ok(await _saleService.UpdateWithUploadAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _saleService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _saleService.DeleteWithUploadAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRangeWithUploadAsync([FromQuery] List<int> id)
        {
            return Ok(await _saleService.DeleteRangeWithUploadAsync(id));
        }
    }
}
