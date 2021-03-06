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
    public interface IPartnerLocalService : IBaseService<PartnerLocalModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<PartnerLocalModel>> GetAllGroupAsync(int partnerId);
        Task<List<PartnerLocalModel>> GetByStatusAsync(bool status);
    }
}
