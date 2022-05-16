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
    [Route("api/recruitment")]
    [ApiController]
    public class RecruitmentController : ControllerBase
    {
        #region Fields
        private readonly IRecruitmentService _recruitmentService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public RecruitmentController(IRecruitmentService recruitmentService, ILogging logging)
        {
            _recruitmentService = recruitmentService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _recruitmentService.GetAllAsync());
        }

        [HttpGet("getallstatusandqty")]
        public async Task<IActionResult> GetAllByStatusAndQty([FromQuery] bool status, [FromQuery] int qty)
        {
            return Ok(await _recruitmentService.GetByStatusAndQtyAsync(status, qty));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _recruitmentService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] RecruitmentModel model)
        {
            return Ok(await _recruitmentService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] RecruitmentModel model)
        {
            return Ok(await _recruitmentService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _recruitmentService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _recruitmentService.DeleteAsync(id));
        }
    }
}
