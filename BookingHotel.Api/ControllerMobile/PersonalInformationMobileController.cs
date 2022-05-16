using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Implementation.Mobile;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.ControllerMobile
{

    public class PersonalInformationMobileController : BaseController
    {
        private readonly IMobileBaseService _service;

        public PersonalInformationMobileController(
            IMobileBaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> NotificationSetting([FromBody] NotificationAccountModel request)
        {
            return Ok(await _service.NotificationSetting(request));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAccountStatus([FromQuery] int accountId)
        {
            return Ok(await _service.UpdateAccountStatus(accountId));
        }
    }
}
