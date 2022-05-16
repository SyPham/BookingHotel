using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IProductWishlistService : IBaseService<ProductWishlistModel>
    {
        Task<ProductWishlistModel> FindById(int accountId, int productId);
        Task<ProcessingResult> DeleteAsync(int accountId, int productId);
        Task<ProcessingResult> DeleteByAccountAsync(int accountId);
        Task<object> GetAllByAccountAsync(int accountId);
    }

}
