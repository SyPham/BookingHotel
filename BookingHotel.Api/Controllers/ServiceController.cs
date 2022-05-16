using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        #region Fields
        private readonly IServiceService _serviceService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogging _logging;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly HttpContext Context;
        #endregion

        #region Ctor
        public ServiceController(IServiceService serviceService,
        IHttpContextAccessor httpContextAccessor,
        ILogging logging,
        IHostingEnvironment currentEnvironment
        )
        {
            _serviceService = serviceService;
            _httpContextAccessor = httpContextAccessor;
            Context = httpContextAccessor.HttpContext;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _serviceService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _serviceService.GetByStatusAsync(status));
        }

        [HttpGet("getallbycategory")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] int catId)
        {
            return Ok(await _serviceService.GetAllByCategoryAsync(catId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _serviceService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _serviceService.FindById(id));
        }

        [HttpGet("gethot")]
        public async Task<IActionResult> GetHot([FromQuery] int NumberItem)
        {
            return Ok(await _serviceService.GetAllHotAsync(NumberItem));
        }

        [HttpGet("getnew")]
        public async Task<IActionResult> GetNew([FromQuery] int NumberItem)
        {
            return Ok(await _serviceService.GetAllNewAsync(NumberItem));
        }

        [HttpGet("getrandom")]
        public async Task<IActionResult> GetRandom([FromQuery] int NumberItem)
        {
            return Ok(await _serviceService.GetAllRandomAsync(NumberItem));
        }

        [HttpGet("gettop")]
        public async Task<IActionResult> GetTop([FromQuery] int NumberItem)
        {
            return Ok(await _serviceService.GetAllTopAsync(NumberItem));
        }

        [HttpGet("getrelated")]
        public async Task<IActionResult> GetRelated([FromQuery] int id, [FromQuery] int catId, [FromQuery] int NumberItem)
        {
            return Ok(await _serviceService.GetAllRelatedAsync(id, catId, NumberItem));
        }

        [HttpPut("updateview")]
        public async Task<IActionResult> UpdateView([FromQuery] int id)
        {
            return Ok(await _serviceService.UpdateViewAsync(id));
        }
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] ServiceModel model)
        {
            return Ok(await _serviceService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ServiceModel model)
        {
            return Ok(await _serviceService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _serviceService.UpdateStatusAsync(id));
        }
        [AllowAnonymous]
        [HttpPost("uploadbox/Save")]
        public async Task<IActionResult> Save([FromQuery] List<IFormFile> UploadFiles)
        {
            IFormFile filesContent = UploadFiles.FirstOrDefault();
            // Ghi file
            var contentUniqueFileName = string.Empty;
            var contentFolderPath = "FileUploads\\images\\service\\content";
            string uploadContentFolder = Path.Combine(_currentEnvironment.WebRootPath, contentFolderPath);

            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadContentFolder);
            var httpContextAccessor = _httpContextAccessor.HttpContext.Response;
            try
            {
                contentUniqueFileName = await fileExtension.WriteAsync(filesContent, $"{uploadContentFolder}");
                Response.AddFileNameHeader(contentUniqueFileName);
                return Ok();
            }
            catch
            {
                Response.AddFileNameHeader(contentUniqueFileName);
                return NoContent();
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _serviceService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _serviceService.DeleteRangeAsync(id));
        }
    }
}
