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
    public interface IServiceCategoryService : IBaseService<ServiceCategoryModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<ServiceCategoryModel>> GetAllGroupAsync(int parentId);
        Task<List<ServiceCategoryModel>> GetByStatusAsync(bool status);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
    }
}
