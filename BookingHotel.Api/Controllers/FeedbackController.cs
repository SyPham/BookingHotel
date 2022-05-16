using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        #region Fields
        private readonly IFeedbackService _feedbackService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public FeedbackController(IFeedbackService feedbackService, ILogging logging)
        {
            _feedbackService = feedbackService;
            _logging = logging;
        }
        #endregion


        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _feedbackService.FindById(id));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _feedbackService.GetByStatusAsync(status));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _feedbackService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _feedbackService.PaginationAsync(paramater));
        }

        [HttpGet("getallstatusandqty")]
        public async Task<IActionResult> GetAllByStatusAndQty(bool status, int qty)
        {
            return Ok(await _feedbackService.GetByStatusAndQtyAsync(status, qty));
        }
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] FeedbackModel model)
        {
            return Ok(await _feedbackService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] FeedbackModel model)
        {
            return Ok(await _feedbackService.UpdateAsync(model));
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _feedbackService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _feedbackService.DeleteRangeAsync(id));
        }
    }
}
