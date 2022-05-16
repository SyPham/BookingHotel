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
    public interface IOrderService : IBaseService<OrderModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id, int status);
        Task<List<OrderModel>> GetByStatusAsync(int status);
        Task<List<OrderModel>> GetByOrderStatusAsync(int orderStatusId);
        Task<List<OrderModel>> GetByPayTypeAsync(int payTypeId);
        Task<List<OrderModel>> GetByCustomerAsync(int customerId);
        Task<Pager> GetByCustomerPaginationAsync(Filter2Pagination paramater, int customerId);
        Task<ProcessingResult> SaveOrder(OrderModel orderModel);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
        Task<ProcessingResult> UpdateOrderStatusAsync(int id, int orderStatusId);

    }
}
