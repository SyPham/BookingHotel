using Microsoft.AspNetCore.Authorization;
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
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.Api.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region Fields
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly ICustomerLiteService _customerLiteService;
        private readonly IAccountLiteService _accountLiteService;
        private readonly ILogging _logging;
        private readonly IWebHostEnvironment _currentEnvironment;
        #endregion

        #region Ctor
        public CustomerController(ICustomerService customerService, ICustomerLiteService customerLiteService, IAccountService accountService, IAccountLiteService accountLiteService, ILogging logging, IWebHostEnvironment currentEnvironment)
        {
            _customerService = customerService;
            _customerLiteService = customerLiteService;
            _accountService = accountService;
            _accountLiteService = accountLiteService;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
        }
        #endregion

        [Authorize]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _customerService.GetByAccountAsync(accountId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _customerService.PaginationAsync(paramater));
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] CustomerLiteModel model)
        {
            ProcessingResult processingResult;

            bool checkEmail = await _customerService.CheckExists(-1, model.Email, 1);
            if (checkEmail)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Email đã tồn tại", Success = false });
            }
            bool checkPhone = await _customerService.CheckExists(-1, model.Phone, 2);
            if (checkPhone)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Số điện thoại đã tồn tại", Success = false });
            }
            if (model == null || model.objAccount == null)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không chính xác", Success = false });
            }
            var account = await _accountService.GetByUsername(model.objAccount.UserName);
            if (account != null)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Tên tài khoản đã tồn tại", Success = false });
            }
            var accountProcess = await _accountLiteService.AddAsync(model.objAccount, 0);
            if (accountProcess.Success)
            {
                // lấy id tài khoản
                var accountNew = (Account)accountProcess.Data;
                processingResult = await _customerLiteService.AddAsync(model, accountNew.Id);
                if (!processingResult.Success)
                    await _accountService.DeleteAsync(accountNew.Id);
            }
            else
            {
                processingResult = accountProcess;
            }
            return Ok(processingResult);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] CustomerLiteModel model)
        {
            ProcessingResult processingResult;
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            var item = await _customerService.GetByAccountAsync(accountId);
            if (item == null)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false });
            }
            bool checkEmail = await _customerService.CheckExists(item.Id, model.Email, 1);
            if (checkEmail)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Email đã tồn tại", Success = false });
            }
            bool checkPhone = await _customerService.CheckExists(item.Id, model.Phone, 2);
            if (checkPhone)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Số điện thoại đã tồn tại", Success = false });
            }
            string uniqueFileName = null;
            string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath, "FileUploads\\Images\\customer");
            IFormFile file = !model.File.IsNullOrEmpty() ? model.File : null;
            if (!file.IsNullOrEmpty())
            {
                if (item.Avatar != null)
                {
                    RemoveFileName($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                }
                uniqueFileName = await RenderFileName(file);
                model.Avatar = $"/FileUploads/Images/customer/{uniqueFileName}";
            }
            processingResult = await _customerLiteService.UpdateAsync(model, accountId);
            if (processingResult.Success)
            {
                return Ok(processingResult);
            }
            if (!uniqueFileName.IsNullOrEmpty())
                RemoveFileName($"{uploadFolder}\\{uniqueFileName}");
            return Ok(processingResult);
        }
 [Authorize]
        [HttpPut("update2")]
        public async Task<IActionResult> Update2([FromBody] CustomerModel model)
        {
            ProcessingResult processingResult;
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            var item = await _customerService.GetByAccountAsync(accountId);
            if (item == null)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false });
            }
            model.Id = item.Id;
            model.AccountId = item.AccountId;
            bool checkEmail = await _customerService.CheckExists(item.Id, model.Email, 1);
            if (checkEmail)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Email đã tồn tại", Success = false });
            }
            bool checkPhone = await _customerService.CheckExists(item.Id, model.Phone, 2);
            if (checkPhone)
            {
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Số điện thoại đã tồn tại", Success = false });
            }
            processingResult = await _customerService.Update2(model);
            if (processingResult.Success)
            {
                return Ok(processingResult);
            }
           
            return Ok(processingResult);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var item = await _customerService.FindById(id);
            if (item == null)
            {
                return NotFound();
            }
            var avatar = item.Avatar;
            var result = await _customerService.DeleteAsync(id);
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
            // Lấy đường dẫn folder
            string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath, "FileUploads\\Images\\customer");
            // Kiểm tra đường dẫn
            bool folderExists = Directory.Exists(uploadFolder);
            if (!folderExists) Directory.CreateDirectory(uploadFolder);
            // Kiểm tra và lưu file
            string uniqueFileName = Guid.NewGuid().ToString() + '_' + file.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            var check = System.IO.File.Exists(Path.Combine(_currentEnvironment.WebRootPath, "/FileUploads/Images/customer/") + uniqueFileName);
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

        [Authorize]
        [HttpPut("UpdateInfo")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateInfoRequest model)
        {
           var processingResult = await _customerService.UpdateInfo(model);
            return Ok(processingResult);
        }
    }
}
