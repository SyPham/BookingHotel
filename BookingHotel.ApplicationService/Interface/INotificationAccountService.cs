using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface INotificationAccountService : IBaseService<NotificationAccountModel>
    {
        Task<NotificationAccountModel> FindById(int accountId, int notificationId);

        Task<ProcessingResult> DeleteAsync(int accountId, int notificationId);
        Task<List<NotificationModel>> GetNotificationByAccount(bool status);

    }

}
