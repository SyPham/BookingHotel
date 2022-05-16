using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;


namespace BookingHotel.ApplicationService.Interface
{
    public interface ICustomerService : IBaseService<CustomerModel>, IBaseUploadService<CustomerModel>
    {
         Task<ProcessingResult> Update2(CustomerModel model);
        Task<CustomerModel> GetByAccountAsync(int accountId);

        CustomerModel GetByAccount(int accountId);
        Task<ProcessingResult> UploadAvatar(UploadAvatarRequest request);
        Task<List<OrderModel>> GetOrderByCustomerId(int customerId);
        Task<List<ProductModel>> GetProductsWishlistByAccountId(int accountId);
        Task<List<ProductModel>> GetPurchasedProductsByCustomerId(int customerId); // lấy danh sách sản phẩm đã mua
        Task<ProcessingResult> UpdatePointAsync(int accountId, int point, int math); // 1 là công, 2 là trừ
        Task<bool> CheckExists(int id, string value, int type); // 1. Email, 2. Số điện thoại

        Task<ProcessingResult> UpdateInfo(UpdateInfoRequest model);

    }
}
