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
    [Route("api/faq")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        #region Fields
        private readonly IFaqService _faqService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public FaqController(IFaqService faqService, ILogging logging)
        {
            _faqService = faqService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _faqService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _faqService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _faqService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] FaqModel model)
        {
            return Ok(await _faqService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] FaqModel model)
        {
            return Ok(await _faqService.UpdateAsync(model));
        }
        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatusAsync([FromQuery] int id)
        {
            return Ok(await _faqService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _faqService.DeleteAsync(id));
        }
    }
}
