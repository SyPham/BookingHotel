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
    public interface IProvinceService : IBaseService<ProvinceModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<ProvinceModel>> GetAllGroupAsync(int parentId);
        Task<List<ProvinceModel>> GetByStatusAsync(bool status);
    }
}
