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
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        #region Fields
        private readonly IContactService _contactService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ContactController(IContactService contactService, ILogging logging)
        {
            _contactService = contactService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _contactService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _contactService.PaginationAsync(paramater));
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _contactService.FindById(id));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contactService.GetAllAsync());
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ContactModel model)
        {
            return Ok(await _contactService.AddAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _contactService.UpdateStatusAsync(id));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ContactModel model)
        {

            return Ok(await _contactService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _contactService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _contactService.DeleteRangeAsync(id));
        }
    }
}
