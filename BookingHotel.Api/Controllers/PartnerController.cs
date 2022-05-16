using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;
using Microsoft.AspNetCore.Authorization;
using BookingHotel.Data.Enums;

namespace BookingHotel.Api.Controllers
{
    [Route("api/partner")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        #region Fields
        private readonly IPartnerService _partnerService;
        private readonly IAccountService _accountService;
        private readonly ILogging _logging;
        private readonly IWebHostEnvironment _currentEnvironment;
        #endregion

        #region Ctor
        public PartnerController(IPartnerService partnerService, IAccountService accountService, ILogging logging, IWebHostEnvironment currentEnvironment)
        {
            _partnerService = partnerService;
            _accountService = accountService;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _partnerService.GetAllAsync());
        }

        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _partnerService.GetByStatusAsync(status));
        }
        [HttpGet("getrelated")]
        public async Task<IActionResult> GetRelated([FromQuery] int id, [FromQuery] int catId, [FromQuery] int NumberItem)
        {
            return Ok(await _partnerService.GetAllRelatedAsync(id, catId, NumberItem));
        }

        [HttpGet("getallgroup")]
        public async Task<IActionResult> GetAllGroup([FromQuery] int typeId)
        {
            return Ok(await _partnerService.GetAllGroupAsync(typeId));
        }
   [HttpGet("getallproductbyaccount")]
        public async Task<IActionResult> GetAllProductByAccount()
        {
                     var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _partnerService.GetAllProductByAccount(accountId));
        }
         [HttpGet("getallproductstopbyaccount")]
        public async Task<IActionResult> GetAllProductStopByAccount()
        {
                     var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _partnerService.GetAllProductStopByAccount(accountId));
        }

         [HttpGet("getallproductissalebyaccount")]
        public async Task<IActionResult> GetAllProductIsSaleByAccount()
        {
                     var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _partnerService.GetAllProductIsSaleByAccount(accountId));
        }
        [HttpGet("getallbyaccount")]
        public async Task<IActionResult> GetAllByAccount([FromQuery] int accountId)
        {
                     var accessToken = Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            accountId = accountId == 0 ? accountID : accountId;
            //
            return Ok(await _partnerService.GetAllByAccountAsync(accountId));
        }
  [HttpGet("getbyaccount")]
        public async Task<IActionResult> GetByAccount()
        {
                  var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _partnerService.GetByAccountAsync(accountId));
        }
        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _partnerService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _partnerService.FindById(id));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] PartnerModel model)
        {
            ProcessingResult processingResult;
            var accountProcess = await _accountService.AddAsync(model.Account);
            if (accountProcess.Success)
            {
                var acc = (Account)accountProcess.Data;
                model.AccountId = acc.Id;
                model.Status = false;
                IFormFile file = model.Files.FirstOrDefault();
                string uniqueFileName = await RenderFileName(file);
                string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath, "FileUploads\\Images\\partner");
                if (!file.IsNullOrEmpty())
                {
                    model.Avatar = $"/FileUploads/Images/partner/{uniqueFileName}";
                }
                processingResult = await _partnerService.AddAsync(model);
                if (!uniqueFileName.IsNullOrEmpty())
                    RemoveFileName($"{uploadFolder}\\{uniqueFileName}");
            }
            else
            {
                processingResult = accountProcess;
            }
            return Ok(processingResult);
        }
        [Authorize]
        [HttpPut("update2")]
        public async Task<IActionResult> Update2([FromBody] PartnerModel model)
        {
            ProcessingResult processingResult;
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            var item = await _partnerService.GetByAccountAsync(accountId);
            if (item == null)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false });
            }
            model.Id = item.Id;
            model.AccountId = item.AccountId;
            
            bool checkEmail = await _partnerService.CheckExists(item.Id, model.Email, 1);
            if (checkEmail)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Email đã tồn tại", Success = false });
            }
            bool checkPhone = await _partnerService.CheckExists(item.Id, model.Phone, 2);
            if (checkPhone)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Số điện thoại đã tồn tại", Success = false });
            }
            processingResult = await _partnerService.Update2(model);
            if (processingResult.Success)
            {
                return Ok(processingResult);
            }
           
            return Ok(processingResult);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PartnerModel model)
        {
            var item = await _partnerService.FindById(model.Id);
            if (item == null)
            {
                return NotFound();
            }

            string uniqueFileName = null;
            string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath, "FileUploads\\Images\\partner");
            IFormFile file = model.Files.FirstOrDefault();
            if (file != null)
            {
                if (item.Avatar != null)
                {
                    RemoveFileName($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                }
                uniqueFileName = await RenderFileName(file);
                model.Avatar = $"/FileUploads/Images/partner/{uniqueFileName}";
            }
            await _accountService.UpdateAsync(model.Account);
            var result = await _partnerService.UpdateAsync(model);
            if (result.Success)
            {
                return NoContent();
            }
            if (!uniqueFileName.IsNullOrEmpty())
                RemoveFileName($"{uploadFolder}\\{uniqueFileName}");
            return BadRequest();
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _partnerService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var item = await _partnerService.FindById(id);
            if (item == null)
            {
                return NotFound();
            }
            var avatar = item.Avatar;
            var result = await _partnerService.DeleteAsync(id);
            if (result.Success)
            {
                if (!avatar.IsNullOrEmpty())
                    RemoveFileName($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");
                return NoContent();
            }

            return BadRequest();
        }

        private async Task<string> RenderFileName(IFormFile file)
        {
            if (file.IsNullOrEmpty())
            {
                return null;
            }
            string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath, "FileUploads\\Images\\partner");
            string uniqueFileName = Guid.NewGuid().ToString() + '_' + file.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            var check = System.IO.File.Exists(Path.Combine(_currentEnvironment.WebRootPath, "/FileUploads/Images/partner/") + uniqueFileName);
            if (check == false)
            {
                using var stream = System.IO.File.Create(filePath);
                await file.CopyToAsync(stream);
                return uniqueFileName;
            }
            else
            {
                return await RenderFileName(file);
            }
        }

        private void RemoveFileName(string path)
        {
            if (path.IsNullOrEmpty() == false)
            {
                var check = System.IO.File.Exists(path);
                if (check)
                {
                    System.IO.File.Delete(path);
                }
            }
        }
    }
}
