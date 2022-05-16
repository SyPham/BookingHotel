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
    [Route("api/tabtype")]
    [ApiController]
    public class TabTypeController : ControllerBase
    {
        #region Fields
        private readonly ITabTypeService _tabtypeService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public TabTypeController(ITabTypeService tabtypeService, ILogging logging)
        {
            _tabtypeService = tabtypeService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetAllByStatus([FromQuery] bool status)
        {
            return Ok(await _tabtypeService.GetByStatusAsync(status));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tabtypeService.GetAllAsync());
        }
    }
}