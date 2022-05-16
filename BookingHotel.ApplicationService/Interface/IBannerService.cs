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
    public interface IBannerService : IBaseService<BannerModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<BannerModel>> GetByStatusAsync(bool status);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
 Task<List<BannerModel>> GetAllRandomAsync(int numberItem);
 Task<List<BannerModel>> GetTopNumberAsync(int numberItem);
 Task<List<BannerModel>> GetMainBanner(int numberItem);
 Task<BannerModel> GetCenterBanner();
    }
}
