using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/abount")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AboutController : ControllerBase
    {
        #region Fields
        private readonly IAboutService _aboutService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public AboutController(IAboutService aboutService, ILogging logging)
        {
            _aboutService = aboutService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _aboutService.GetAllAsync());
        }
    [HttpGet("getfirst")]
        public async Task<IActionResult> GetFisrtAsync()
        {
            return Ok(await _aboutService.GetFisrtAsync());
        }
        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _aboutService.PaginationAsync(paramater));
        }


        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _aboutService.FindById(id));
        }


        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] AboutModel model)
        {

            return Ok(await _aboutService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] AboutModel model)
        {

            return Ok(await _aboutService.UpdateAsync(model));
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _aboutService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _aboutService.DeleteRangeAsync(id));
        }
    }
}
