using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/tab")]
    [ApiController]
    public class TabController : ControllerBase
    {
        #region Fields
        private readonly ITabService _tabService;
        private readonly IProductTabService _productTabService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public TabController(ITabService tabService, IProductTabService productTabService, ILogging logging)
        {
            _tabService = tabService;
            _productTabService = productTabService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tabService.GetAllAsync());
        }

        [HttpGet("getbyproduct")]
        public async Task<IActionResult> GetByProduct([FromQuery] int productId)
        {
            return Ok(await _tabService.GetByProductAsync(productId));
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _tabService.GetByStatusAsync(status));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _tabService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _tabService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] TabModel model)
        {
            return Ok(await _tabService.AddAsync(model));
        }

        [HttpPost("insertbyproduct")]
        public async Task<IActionResult> InsertByProduct([FromBody] TabModel model)
        {
            var tabProcess = await _tabService.AddAsync(model);
            if (tabProcess.Success)
            {
                var tab = (Tab)tabProcess.Data;
                ProductTabModel productTabmodel = new ProductTabModel();
                productTabmodel.TabId = tab.Id;
                productTabmodel.ProductId = (int)model.ProductId;
                productTabmodel.Value = model.Value;
                productTabmodel.CreateTime = DateTime.Now;
                await _productTabService.AddAsync(productTabmodel);
            }
            return Ok(tabProcess);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TabModel model)
        {

            return Ok(await _tabService.UpdateAsync(model));
        }

        [HttpPut("updatebyproduct")]
        public async Task<IActionResult> UpdateByProduct([FromBody] TabModel model)
        {

            var tabProcess = await _tabService.UpdateAsync(model);
            if (tabProcess.Success)
            {
                var tab = (Tab)tabProcess.Data;
                ProductTabModel productTabmodel = new ProductTabModel();
                productTabmodel.TabId = tab.Id;
                productTabmodel.ProductId = (int)model.ProductId;
                productTabmodel.Value = model.Value;
                productTabmodel.CreateTime = DateTime.Now;
                await _productTabService.UpdateAsync(productTabmodel);
            }
            return Ok(tabProcess);
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _tabService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _tabService.DeleteAsync(id));
        }
    }
}
