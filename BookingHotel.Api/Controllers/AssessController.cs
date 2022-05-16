using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Api.Controllers
{
    [Route("api/assess")]
    [ApiController]
    public class AssessController : ControllerBase
    {
        #region Fields
        private readonly IAssessService _assessService;
        private readonly IProductService _productService;
        private readonly IServiceService _serviceService;
        private readonly IArticleService _articleService;
        private readonly IPartnerService _partnerService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public AssessController(IAssessService assessService,
                                IProductService productService,
                                IServiceService serviceService,
                                IArticleService articleService,
                                IPartnerService partnerService,
                                ILogging logging)
        {
            _assessService = assessService;
            _productService = productService;
            _serviceService = serviceService;
            _articleService = articleService;
            _partnerService = partnerService;
            _logging = logging;
        }
        #endregion

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _assessService.GetAllAsync());
        }
 [HttpGet("getallbykey")]
        public async Task<IActionResult> GetAllByKey(int keyId, string keyName )
        {
            return Ok(await _assessService.GetAllByKey(keyId, keyName));
        }

        [HttpGet("getbykeypagination")]
        public async Task<IActionResult> GetAllByKey([FromQuery] KeyPagination paramater)
        {
            return Ok(await _assessService.GetAllByKeyPaginationAsync(paramater, "Product"));
        }

        [Authorize]
        [HttpGet("getbyaccountpagination")]
        public IActionResult GetAllByAccount([FromQuery] ParamaterPagination paramater)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(_assessService.GetAllByAccountPagination(paramater, "Product", accountId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _assessService.PaginationAsync(paramater));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _assessService.FindById(id));
        }

        [Authorize]
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] AssessModel model)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            model.AccountId = accountId;
            //
            int keyId = model.KeyId.Value.ToInt();
            string keyName = model.KeyName;

            if (keyId < 1 || (keyName != "Product" && keyName != "Service" && keyName != "Article" && keyName != "Partner"))
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Nội dung không tìn tại", Success = false });
            //
            var assessResult = await _assessService.AddAsync(model);
            decimal[] mathResult = await _assessService.GetMathByKeyAsync(model.KeyId.Value, model.KeyName);
            decimal avg = mathResult[0];
            decimal count = mathResult[1];

            if (keyName == "Product")
            {
                var item = await _productService.FindByIdNoTracking(keyId);
                if (item != null)
                {
                    //item.TotalAssess = (int)count;
                    //item.ValueAssess = avg;
                    await _productService.UpdateValueAssessAsync(new ValueAssessRequest
                    {   Id = item.Id,
                        ValueAssess = avg,
                        TotalAssess = (int)count
                    });
                }
            }
            else if (keyName == "Service")
            {
                var item = await _serviceService.FindByIdNoTracking(keyId);
                if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _serviceService.UpdateAsync(item);
                }
            }
            else if (keyName == "Article")
            {
                var item = await _articleService.FindByIdNoTracking(keyId); if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _articleService.UpdateAsync(item);
                }
            }
            else if (keyName == "Partner")
            {
                var item = await _partnerService.FindByIdNoTracking(keyId); if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _partnerService.UpdateAsync(item);
                }
            }
            else { }

            return Ok(assessResult);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AssessModel model)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            model.AccountId = accountId;
            //
            if (model.Id < 1)
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Nội dung không tìn tại", Success = false });
            //
            int keyId = model.KeyId.Value.ToInt();
            string keyName = model.KeyName;

            if (keyId < 1 || (keyName != "Product" && keyName != "Service" && keyName != "Article" && keyName != "Partner"))
                return Ok(new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Nội dung không tìn tại", Success = false });
            //
            var assessResult = await _assessService.UpdateAsync(model);
            // Lấy kết quả tính được
            decimal[] mathResult = await _assessService.GetMathByKeyAsync(model.KeyId.Value, model.KeyName);
            decimal avg = mathResult[0]; // trung bình sao
            decimal count = mathResult[1]; // tổng số bình luận

            if (keyName == "Product")
            {
                var item = await _productService.FindByIdNoTracking(keyId);
                if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _productService.UpdateAsync(item);
                }
            }
            else if (keyName == "Service")
            {
                var item = await _serviceService.FindByIdNoTracking(keyId);
                if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _serviceService.UpdateAsync(item);
                }
            }
            else if (keyName == "Article")
            {
                var item = await _articleService.FindByIdNoTracking(keyId); if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _articleService.UpdateAsync(item);
                }
            }
            else if (keyName == "Partner")
            {
                var item = await _partnerService.FindByIdNoTracking(keyId); if (item != null)
                {
                    item.TotalAssess = (int)count;
                    item.ValueAssess = avg;
                    await _partnerService.UpdateAsync(item);
                }
            }
            else { }

            return Ok(assessResult);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _assessService.DeleteAsync(id));
        }
    }
}
