using System.Collections.Generic;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IOrderDetailService : IBaseService<OrderDetailModel>
    {
        Task<OrderDetailModel> FindById(int orderId, int productId);
        Task<ProcessingResult> DeleteByOrderAsync(int orderId);
        Task<ProcessingResult> DeleteByKeyAsync(int orderId, int productId);
        Task<List<OrderDetailModel>> GetByOrderAsync(int orderId);
    }
}
