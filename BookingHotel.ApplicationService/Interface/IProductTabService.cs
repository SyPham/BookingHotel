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
    public interface IProductTabService : IBaseService<ProductTabModel>
    {
        Task<ProductTabModel> FindById(int productId, int tabId);

        Task<ProcessingResult> DeleteAsync(int productId, int tabId);
    }
}
