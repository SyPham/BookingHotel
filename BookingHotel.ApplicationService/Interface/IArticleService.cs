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
    public interface IArticleService : IBaseService<ArticleModel>
    {
        Task<ArticleModel> FindByIdNoTracking(int id);
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<ArticleModel>> GetAllByCategoryAsync(int catId);
        Task<List<ArticleModel>> GetByStatusAsync(bool status);
        Task<List<ArticleModel>> GetAllRelatedAsync(int id, int catID, int numberItem);
        Task<List<ArticleModel>> GetAllNewAsync(int numberItem);
        Task<List<ArticleModel>> GetAllHotAsync(int numberItem);
        Task<List<ArticleModel>> GetAllRandomAsync(int numberItem);
        Task<List<ArticleModel>> GetAllTopAsync(int numberItem);
        Task<ProcessingResult> UpdateViewAsync(int id);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);

    }
}
