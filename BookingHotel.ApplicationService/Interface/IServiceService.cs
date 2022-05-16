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
    public interface IServiceService : IBaseService<ServiceModel>
    {
        Task<ServiceModel> FindByIdNoTracking(int id);
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<ServiceModel>> GetAllByCategoryAsync(int catId);
        Task<List<ServiceModel>> GetByStatusAsync(bool status);
        Task<List<ServiceModel>> GetAllRelatedAsync(int id, int catID, int numberItem);
        Task<List<ServiceModel>> GetAllNewAsync(int numberItem);
        Task<List<ServiceModel>> GetAllHotAsync(int numberItem);
        Task<List<ServiceModel>> GetAllRandomAsync(int numberItem);
        Task<List<ServiceModel>> GetAllTopAsync(int numberItem);
        Task<ProcessingResult> UpdateViewAsync(int id);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);

    }
}
