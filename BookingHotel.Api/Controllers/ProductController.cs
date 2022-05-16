using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
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
    [Route("api/product")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly ILogging _logging;
        #endregion

        #region Ctor
        public ProductController(IProductService productService, ILogging logging)
        {
            _productService = productService;
            _logging = logging;
        }
        #endregion
 [HttpGet("getallproductwallet")]
        public async Task<IActionResult> GetAllProductByAccount()
        {
               var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productService.GetAllProductByAccount(accountId ));
        }
[HttpGet("getallproductwalletisuse")]
        public async Task<IActionResult> GetAllProductIsUseByAccount()
        {
               var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productService.GetAllProductIsUseByAccount(accountId ));
        }
        [HttpGet("getallproductwalletnotuse")]
        public async Task<IActionResult> GetAllProductNotUseByAccount()
        {
               var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productService.GetAllProductNotUseByAccount(accountId ));
        }
        [HttpPost("loaddata")]
        public async Task<IActionResult> LoadData([FromBody]FilterRequest request)
        {
            return Ok(await _productService.LoadData(request ));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAllAsync());
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(int catId, string keyWord)
        {
            return Ok(await _productService.Search(catId,keyWord));
        }
        [HttpGet("getallstatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] bool status)
        {
            return Ok(await _productService.GetByStatusAsync(status));
        }

        [HttpGet("getallbycategory")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] int catId)
        {
            return Ok(await _productService.GetAllByCategoryAsync(catId));
        }

        [HttpGet("getallbysale")]
        public async Task<IActionResult> GetAllBySaleId([FromQuery] int saleId)
        {
            return Ok(await _productService.GetAllBySaleAsync(saleId));
        }

        [HttpGet("getallbyuntil")]
        public async Task<IActionResult> GetAllByUntilId([FromQuery] int untilId)
        {
            return Ok(await _productService.GetAllByUntilAsync(untilId));
        }

        [HttpGet("getallbypartner")]
        public async Task<IActionResult> GetAllByParterId([FromQuery] int partnerId)
        {
            return Ok(await _productService.GetAllByPartnerAsync(partnerId));
        }

        [HttpGet("getallpagination")]
        public async Task<IActionResult> GetAllPagination([FromQuery] ParamaterPagination paramater)
        {
            return Ok(await _productService.PaginationAsync(paramater));
        }

        [HttpGet("searchpagination")]
        public async Task<IActionResult> SearchPagination([FromQuery] SearchPagination paramater)
        {
            return Ok(await _productService.SearchPaginationAsync(paramater));
        }

        [HttpGet("filterpagination")]
        public async Task<IActionResult> FilterPagination([FromQuery] FilterPagination paramater)
        {
            return Ok(await _productService.FilterPaginationAsync(paramater));
        }

        [Authorize]
        [HttpGet("wishlistpagination")]
        public async Task<IActionResult> WishlistPagination([FromQuery] WishlistPagination paramater)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productService.WishlistPaginationAsync(paramater, accountId));
        }
  [Authorize]
        [HttpGet("getwishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productService.GetWishlistAsync(accountId));
        }
        [Authorize]
        [HttpGet("getwishlistbyid")]
        public async Task<IActionResult> IsWishlistFindById([FromQuery] int id)
        {
            var accessToken = Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            return Ok(await _productService.IsWishlistFindById(id, accountId));
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByID([FromQuery] int id)
        {
            return Ok(await _productService.FindById(id));
        }

        [HttpGet("gethot")]
        public async Task<IActionResult> GetHot([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllHotAsync(NumberItem));
        }

        [HttpGet("getnew")]
        public async Task<IActionResult> GetNew([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllNewAsync(NumberItem));
        }

        [HttpGet("getrandom")]
        public async Task<IActionResult> GetRandom([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllRandomAsync(NumberItem));
        }

        [HttpGet("gettop")]
        public async Task<IActionResult> GetTop([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllTopAsync(NumberItem));
        }
        [HttpGet("gettopsale")]
        public async Task<IActionResult> GetAllSaleAsync([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllSaleAsync(NumberItem));
        }
        [HttpGet("getrelated")]
        public async Task<IActionResult> GetRelated([FromQuery] int id, [FromQuery] int catId, [FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllRelatedAsync(id, catId, NumberItem));
        }

        [HttpPut("updateview")]
        public async Task<IActionResult> UpdateView([FromQuery] int id)
        {
            return Ok(await _productService.UpdateViewAsync(id));
        }
        [HttpPut("updatevalueassess")]
        public async Task<IActionResult> UpdateValueAssessAsync([FromBody] ValueAssessRequest request)
        {
            return Ok(await _productService.UpdateValueAssessAsync(request));
        }
        [HttpGet("getsalebycat")]
        public async Task<IActionResult> GetSaleByCategoryAsync([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetSaleByCategoryAsync(NumberItem));
        }
        [HttpGet("GetAllBestSeller")]
        public async Task<IActionResult> GetAllBestSellerAsync([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetAllBestSellerAsync(NumberItem));
        }
        [HttpGet("GetFeaturedProducts")]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] int NumberItem)
        {
            return Ok(await _productService.GetFeaturedProducts(NumberItem));
        }
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromForm] ProductActionModel model)
        {
            var tabRequest = Request.Form["tabRequest"].ToString();

            var result = JsonConvert.DeserializeObject<List<TabRequest>>(tabRequest);
            model.TabRequest = result;
            return Ok(await _productService.AddAsync(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ProductActionModel model)
        {
            var tabRequest = Request.Form["tabRequest"].ToString();

            var result = JsonConvert.DeserializeObject<List<TabRequest>>(tabRequest);
            model.TabRequest = result;
            return Ok(await _productService.UpdateAsync(model));
        }

        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id)
        {
            return Ok(await _productService.UpdateStatusAsync(id));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await _productService.DeleteAsync(id));
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> id)
        {
            return Ok(await _productService.DeleteRangeAsync(id));
        }

        [HttpGet("FilterNameAZ")]
        public async Task<IActionResult> FilterNameAZ([FromQuery] int catId)
        {
            return Ok(await _productService.FilterNameAZ(catId));
        }
        [HttpGet("FilterNameZA")]
        public async Task<IActionResult> FilterNameZA([FromQuery] int catId)
        {
            return Ok(await _productService.FilterNameZA(catId));
        }

        [HttpGet("FilterPriceAZ")]
        public async Task<IActionResult> FilterPriceAZ([FromQuery] int catId)
        {
            return Ok(await _productService.FilterPriceAZ(catId));
        }
        [HttpGet("FilterPriceZA")]
        public async Task<IActionResult> FilterPriceZA([FromQuery] int catId)
        {
            return Ok(await _productService.FilterPriceZA(catId));
        }
        [HttpGet("FilterRatingAZ")]
        public async Task<IActionResult> FilterRatingAZ([FromQuery] int catId)
        {
            return Ok(await _productService.FilterRatingAZ(catId));
        }
        [HttpGet("FilterRatingZA")]
        public async Task<IActionResult> FilterRatingZA([FromQuery] int catId)
        {
            return Ok(await _productService.FilterRatingZA(catId));
        }
    }
}
