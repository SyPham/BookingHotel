using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;
using BookingHotel.Web.ClassHelpers;
using X.PagedList;
using static BookingHotel.Web.TagHelpers.MessageTagHelper;
using BookingHotel.Common.Helpers;

namespace BookingHotel.Web.Controllers
{
    public class ProductController : Controller
    {
        #region Fields
        private readonly ILogging _logging;
        private readonly IOptions<Config> _config;
        private readonly HttpClient client;
        #endregion

        #region Ctor
        public ProductController(IHttpClientFactory clientFactory, ILogging logging, IOptions<Config> config)
        {
            client = clientFactory.CreateClient("default");
            _logging = logging;
            _config = config;
        }
        #endregion
        [Route("/san-pham/{page?}/{catid?}/{title?}", Name = "product")]
        public async Task<IActionResult> Index(int? page, int catID = 0, string title = "")
        {
            var responsePartner = await client.GetAsync($"api/partner/getallstatus?status=true");

            if (responsePartner == null)
                return View();

            if (responsePartner.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string jsonPartner = responsePartner.Content.ReadAsStringAsync().Result;
            List<PartnerModel> dataPartner = JsonConvert.DeserializeObject<List<PartnerModel>>(jsonPartner);
            ViewBag.Partner = dataPartner;
            //Khai báo pageSize dùng chung
            int pageNumper = 0;
            int.TryParse(page.ToString(), out pageNumper);
            if (pageNumper == 0)
                pageNumper = 1;
            //Khai báo url đến api cần gọi
            string apiUrl = string.Empty; 
            if (catID > 0)
            {
                apiUrl = $"api/product/getallbycategory?catId={catID}";
                var responseCat = await client.GetAsync($"api/productcategory/getbyid?id={catID}");
                string jsonCat = responseCat.Content.ReadAsStringAsync().Result;
                if (responseCat == null)
                    return View();

                if (responseCat.StatusCode != System.Net.HttpStatusCode.OK)
                    return View();
                ProductCategoryModel dataCat = JsonConvert.DeserializeObject<ProductCategoryModel>(jsonCat);
               ViewBag.ProductCat = dataCat;
            }
            else
            {
                apiUrl = $"api/product/getallstatus?status=true";
            }
            var response = await client.GetAsync(apiUrl);

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<ProductModel> dataTemp = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            IPagedList<ProductModel> data = dataTemp.ToPagedList(pageNumper, 3);
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.Id = catID;

            return View(data);
        }
        [Route("/danh-sach-san-pham", Name = "product-default")]
        public async Task<IActionResult> List()
        {
            var responsePartner = await client.GetAsync($"api/partner/getallstatus?status=true");

            if (responsePartner == null)
                return View();

            if (responsePartner.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string jsonPartner = responsePartner.Content.ReadAsStringAsync().Result;
            List<PartnerModel> dataPartner = JsonConvert.DeserializeObject<List<PartnerModel>>(jsonPartner);
            ViewBag.Partner = dataPartner;

            var responseProductCategory = await client.GetAsync($"api/productcategory/getallstatus?status=true");

            if (responseProductCategory == null)
                return View();

            if (responseProductCategory.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string jsonProductCategory = responseProductCategory.Content.ReadAsStringAsync().Result;
            var dataProductCategory = JsonConvert.DeserializeObject<List<ProductCategoryModel>>(jsonProductCategory);
            ViewBag.ProductCategory = dataProductCategory;

         
            ViewBag.Url = _config.Value.ApiUrl;

            return View();
        }
        [Route("/san-pham/chi-tiet/{id?}/{catid?}/{title?}", Name = "product-detail")]
        public async Task<IActionResult> Detail(int ID = 0, int catID = 0, string title = "")
        {
            var response = await client.GetAsync($"api/product/getbyid?id={ID}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();
            string json = response.Content.ReadAsStringAsync().Result;
            ProductModel data = JsonConvert.DeserializeObject<ProductModel>(json);

            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.ID = ID;
            ViewBag.CatID = catID;
            if (User.Identity.Name.ToNullableInt() != null )
            {
                var response3 = await client.GetAsync($"api/customer/getbyid");

                if (response3 == null)
                    return View();

                if (response3.StatusCode != System.Net.HttpStatusCode.OK)
                    return View();
                string json3 = response3.Content.ReadAsStringAsync().Result;
                CustomerModel data3 = JsonConvert.DeserializeObject<CustomerModel>(json3);
                ViewBag.Customer = data3;
            }
            return View(data);
        }
        [Route("/tim-kiem/{page?}/{keyword?}", Name = "product-search")]
        public async Task<IActionResult> Search(int? page, string keyword = "")
        {
            //Khai báo pageSize dùng chung
            int pageNumper = 0;
            int.TryParse(page.ToString(), out pageNumper);
            if (pageNumper == 0)
                pageNumper = 1;
            var response = await client.GetAsync($"api/product/Search?catId=0&keyWord={keyword}");
            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<ProductModel> dataTemp = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            IPagedList<ProductModel> data = dataTemp.ToPagedList(pageNumper, 20);
            ViewBag.Url = _config.Value.ApiUrl;
            return View(data);
        }
        [Route("/cua-hang/{catId}/{page?}/{pageSize?}/{sortBy?}", Name = "shop")]
        public async Task<IActionResult> Shop(int catId = 0, int page = 1, int pageSize = 24, int sortBy = 0)
        {
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SortBy = sortBy;
            ViewBag.CatId = catId;
            var response = await client.GetAsync($"api/productcategory/GetBreadcrumbById?id={catId}");
            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<HierarchyNode<MainMenuModel>> dataTemp = JsonConvert.DeserializeObject<List<HierarchyNode<MainMenuModel>>>(json);
          
            return View(dataTemp);
        }
        public async Task<ActionResult> FilterResult(int catId, int? page, int pageSize, int sortBy)
        {
            //Khai báo pageSize dùng chung
            int pageNumper = 0;
            int.TryParse(page.ToString(), out pageNumper);
            if (pageNumper == 0)
                pageNumper = 1;
            string uri = "";
            switch ((SortBy)sortBy)
            {
                case SortBy.Rating_AZ:
                    uri = "filterratingaz";
                    break;
                case SortBy.Rating_ZA:
                    uri = "filterratingza";

                    break;
                case SortBy.Name_AZ:
                    uri = "filternameaz";
                    break;
                case SortBy.Name_ZA:
                    uri = "filternameaz";
                    break;
                case SortBy.Price_AZ:
                    uri = "filterpriceaz";
                    break;
                case SortBy.Price_ZA:
                    uri = "filterpriceza";
                    break;
                default:
                    break;
            }
            var response = await client.GetAsync($"api/product/{uri}?catId={catId}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            List<ProductModel> dataTemp = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            IPagedList<ProductModel> data = dataTemp.ToPagedList(pageNumper, pageSize);
            ViewBag.Url = _config.Value.ApiUrl;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SortBy = sortBy;
            return PartialView(data);
        }
        [HttpPost]
        public async Task<IActionResult> Loaddata([FromBody]FilterRequest request)
        {
            //Ép kiểu json(Bước xử lý trung gian)
            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response2 = await client.PostAsync($"api/product/loaddata", stringContent);
            if (response2 == null)
                return Ok(new {
                    data = new List<dynamic>(),
                    total = 0,
                    status = false
                });

            if (response2.StatusCode != System.Net.HttpStatusCode.OK)
                return Ok(new {
                    data = new List<dynamic>(),
                    total = 0,
                    status = false
                });
            string json2 = response2.Content.ReadAsStringAsync().Result;
            // Xử lý khi đặt hàng
            return Ok(json2);
        }

        /// Thêm sản phẩm vào cart
        [Route("them-gio-hang/{productid:int}", Name = "addcart")]
        public async Task<IActionResult> AddToCart([FromRoute] int productid)
        {
            var returnUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            if (User.Identity.Name == null) return Redirect($"/dang-nhap?returnUrl={returnUrl}");
            var item = new CartModel { ProductId = productid , AccountId = User.Identity.Name.ToInt()};
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/cart/additemintocart", stringContent);

            if (response == null)
                return RedirectToAction(nameof(Cart));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return RedirectToAction(nameof(Cart));

            return RedirectToAction(nameof(Cart));
        }
        [HttpPost]
        [Route("them-gio-hang/{productid:int}/{quantity:int}", Name = "addcartdetail")]
        public async Task<IActionResult> AddToCartDetail([FromRoute] int productid, [FromRoute] int quantity)
        {
            var returnUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            if (User.Identity.Name == null) return Json(new { Status = false });
            var item = new CartModel { ProductId = productid, AccountId = User.Identity.Name.ToInt(), Quantity = quantity };
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/cart/additemintocart", stringContent);

            if (response == null)
                return Json(new { Status = false });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return Json(new { Status = false });

            return Json(new { Status = true });
        }
        /// xóa item trong cart
        [Route("/xoa-gio-hang/{productid:int}", Name = "removecart")]
        public async Task<IActionResult> RemoveCart([FromRoute] int productid)
        {
            var response = await client.DeleteAsync($"api/cart/deletecartitem?productId={productid}");

            if (response == null)
                return RedirectToAction(nameof(Cart));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return RedirectToAction(nameof(Cart));
            string json = response.Content.ReadAsStringAsync().Result;
            var carts = JsonConvert.DeserializeObject<List<CartModel>>(json);

            return RedirectToAction(nameof(Cart));
        }
        /// Cập nhật
        [HttpPut]
        [Route("/cap-nhap-so-luong", Name = "updatecart")]
        public async Task<IActionResult> UpdateCart( int productid, int quantity)
        {
            var response = await client.PutAsync($"api/cart/updatecartitemquantity?productId={productid}&quantity={quantity}", null);

            if (response == null)
                return RedirectToAction(nameof(Cart));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return RedirectToAction(nameof(Cart));
            return Json(new { Status = true});
        }
     
        // Hiện thị giỏ hàng
        [Route("/gio-hang", Name = "cart")]
        public async Task<IActionResult> Cart()
        {
            ViewBag.Url = _config.Value.ApiUrl;
            var response = await client.GetAsync($"api/cart/getcartitems");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<CartModel>>(json);
            return View(data);
        }
       

        [Route("/dat-hang", Name = "checkout")]
        public async Task<IActionResult> CheckOut()
        {
            ViewBag.Url = _config.Value.ApiUrl;
            var accountId = User.Identity.Name;
            var response = await client.GetAsync($"api/customer/getbyid?id={accountId.ToInt()}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            CustomerModel data = JsonConvert.DeserializeObject<CustomerModel>(json);
            OrderModel model = new OrderModel
            {
                FullName = data.FullName,
                Email = data.Email,
                Mobi = data.Phone
            };
            return View(model);
        }

        [Route("/dat-hang", Name = "checkout")]
        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderModel orderModel)
        {

            #region Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(orderModel.FullName))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập họ tên";
                return View();
            }

            if (string.IsNullOrWhiteSpace(orderModel.Email))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập email";
                return View();
            }
           
            if (string.IsNullOrWhiteSpace(orderModel.Mobi))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập số điện thoại";
                return View();
            }

            if (string.IsNullOrWhiteSpace(orderModel.Address))
            {
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Vui lòng nhập địa chỉ";
                return View();
            }
            #endregion

            ViewBag.Url = _config.Value.ApiUrl;
            var accountId = User.Identity.Name;
            var response = await client.GetAsync($"api/customer/getbyid?id={accountId.ToInt()}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            CustomerModel data = JsonConvert.DeserializeObject<CustomerModel>(json);
            orderModel.PayTypeId = 1; // Dung diem
            orderModel.OrderStatusId = 1; // Cho thanh toan
            orderModel.IsPoint =  true; // Cho thanh toan
            orderModel.CustomerId = data.Id; 
            orderModel.CreateBy = accountId.ToInt();
            orderModel.CreateTime = DateTime.Now;
            //Ép kiểu json(Bước xử lý trung gian)
            var jsonOrder = JsonConvert.SerializeObject(orderModel);
            var stringContentOrder = new StringContent(jsonOrder, UnicodeEncoding.UTF8, "application/json");

            var responseOrder = await client.PostAsync("api/order/saveorder", stringContentOrder); //Gửi lên server Post async

            if (responseOrder == null)
                return View();

            if (responseOrder.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //Hiện thông báo lỗi
                #region Hiển thị thông báo
                ViewBag.Type = MessageType.danger;
                ViewBag.Content = "Đã xảy ra lỗi. Vui lòng thử lại";
                #endregion
                return View();
            }
            else
            {
                //Hiện chúc mừng

                #region Hiển thị thông báo
                ViewBag.Type = MessageType.success;
                ViewBag.Content = "Đặt hàng thành công.";
                #endregion

                //Xóa trắng form
                ModelState.Remove("Address");
                ModelState.Remove("Remark");

                //Trả về kết quả
                return View();
            }
        }
        [Route("/yeu-thich/{page?}", Name = "wish-list")]
        public async Task<IActionResult> WishList(int? page)
        {
            //Khai báo url đến api cần gọi
            var response = await client.GetAsync($"api/product/getwishlist");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
           
            //Khai báo pageSize dùng chung
            int pageNumper = 0;
            int.TryParse(page.ToString(), out pageNumper);
            if (pageNumper == 0)
                pageNumper = 1;
            //Khai báo url đến api cần gọi
            
            List<ProductModel> dataTemp = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            IPagedList<ProductModel> data = dataTemp.ToPagedList(pageNumper, 3);
            ViewBag.Url = _config.Value.ApiUrl;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> ProductWishlist(int productID)
        {
            //Ép kiểu json(Bước xử lý trung gian)
            var item = new ProductWishlistLiteModel { ProductId = productID };
            var json = JsonConvert.SerializeObject(item);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("api/productwishlist/insert", stringContent); //Gửi lên server Post async

            if (response == null)
                return Json(new { Status = false });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return Json(new { Status = false });

            //tra ve neu chưa login
            return Json(new { Status = true });
        }

        [HttpPost]
        public async Task<ActionResult> DeteleProductWishlist(int productID)
        {
            var response = await client.DeleteAsync($"api/productwishlist/delete?productId={productID}"); //Gửi lên server Delete Async

            if (response == null)
                return Json(new { Status = false });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return Json(new { Status = false });

            //tra ve neu chưa login
            return Json(new { Status = true });
        }

        public async Task<ActionResult> QuickView(int ID)
        {
            var response = await client.GetAsync($"api/product/getbyid?id={ID}");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();


            string json = response.Content.ReadAsStringAsync().Result;
            ProductModel data = JsonConvert.DeserializeObject<ProductModel>(json);

            ViewBag.Url = _config.Value.ApiUrl;

            return PartialView(data);
        }

        public async Task<ActionResult> ProductRating(int ID)
        {
            var response = await client.GetAsync($"api/assess/getallbykey?keyId={ID}&keyName=Product");

            if (response == null)
                return View();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return View();

            string json = response.Content.ReadAsStringAsync().Result;
            List<AssessWebModel> data = JsonConvert.DeserializeObject<List<AssessWebModel>>(json);
            ViewBag.Url = _config.Value.ApiUrl;
            return PartialView(data);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateValueAssess(ValueAssessRequest request)
        {
            //Ép kiểu json(Bước xử lý trung gian)
            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PutAsync("api/product/UpdateValueAssess", stringContent); //Gửi lên server Post async

            if (response == null)
                return Json(new { Status = false });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return Json(new { Status = false });

            return Json(new { Status = true });
        }
        [HttpPost]
        public async Task<ActionResult> AddAssess(AssessModel request)
        {
            //Ép kiểu json(Bước xử lý trung gian)
            var json = JsonConvert.SerializeObject(request);
            request.CreateTime = DateTime.Now;
            var id = User.Identity.Name.ToNullableInt();
            request.AccountId = id;
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("api/assess/insert", stringContent); //Gửi lên server Post async

            if (response == null)
                return Json(new { Status = false });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return Json(new { Status = false });

            return Json(new { Status = true });
        }
        [HttpPut]
        public async Task<ActionResult> UpdateView(int id)
        {
            var response = await client.PutAsync("api/product/updateview?id=" + id, null);

            if (response == null)
                return Json(new { Status = false });

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return Json(new { Status = false });

            return Json(new { Status = true });
        }
    }
}
