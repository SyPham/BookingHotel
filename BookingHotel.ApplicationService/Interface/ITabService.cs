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
    public interface ITabService : IBaseService<TabModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<TabModel>> GetAllGroupAsync(int parentId);
        Task<List<TabModel>> GetByStatusAsync(bool status);
        Task<List<TabModel>> GetByProductAsync(int productId);
    }
}
