using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;


namespace BookingHotel.ApplicationService.Interface
{
    public interface INotificationTypeService : IBaseService<NotificationTypeModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<NotificationTypeModel>> GetByStatusAsync(bool status);
    }
}
