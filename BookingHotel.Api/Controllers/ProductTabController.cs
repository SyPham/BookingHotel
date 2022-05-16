using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/producttab")]
    [ApiController]
    public class ProductTabController : ControllerBase
    {
        #region Fields
        private readonly IProductTabService _productTabService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ProductTabController(IProductTabService productTabService, ILogging logging)
        {
            _productTabService = productTabService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productTabService.GetAllAsync());
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _productTabService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _productTabService.FindById(id));
        }

        [HttpGet("getbymultiid")]
        public async Task<IActionResult> GetByID([FromQuery] int productId, [FromQuery] int tabId)
        {
            return Ok(await _productTabService.FindById(productId, tabId));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] ProductTabModel model)
        {
            
            return Ok(await _productTabService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ProductTabModel model)
        {
           
            return Ok(await _productTabService.UpdateAsync(model));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int productId, [FromQuery] int tabId)
        {
            return Ok(await _productTabService.DeleteAsync(productId, tabId));
        }
    }
}
