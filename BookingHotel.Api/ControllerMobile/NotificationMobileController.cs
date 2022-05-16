using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Implementation;
using BookingHotel.ApplicationService.Implementation.Mobile;
using BookingHotel.ApplicationService.Interface;

namespace BookingHotel.Api.ControllerMobile
{
   
    public class NotificationMobileController : BaseController
    {
        private readonly IMobileBaseService _service;

        public NotificationMobileController(
            IMobileBaseService service
            )
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllNotificationAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _service.GetByIDNotificationAsync(id));
        }
    }
}
