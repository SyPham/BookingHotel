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
    public interface IArticleCategoryService : IBaseService<ArticleCategoryModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<ArticleCategoryModel>> GetAllGroupAsync(int parentId);
        Task<List<ArticleCategoryModel>> GetByStatusAsync(bool status);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
        Task<ProcessingResult> UpdateStatus(int id);
    }
}
