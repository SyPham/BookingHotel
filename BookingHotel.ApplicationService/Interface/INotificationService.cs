using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;


namespace BookingHotel.ApplicationService.Interface
{
    public interface INotificationService : IBaseService<NotificationModel>
    {
        Task<Pager> GetByTypePaginationAsync(TypePagination paramater);
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<NotificationModel>> GetByStatusAsync(bool status);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
    }
}
