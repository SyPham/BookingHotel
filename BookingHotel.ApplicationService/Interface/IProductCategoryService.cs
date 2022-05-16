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
    public interface IProductCategoryService : IBaseService<ProductCategoryModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<ProductCategoryModel>> GetAllGroupAsync(int parentId);
        Task<List<ProductCategoryModel>> GetByStatusAsync(bool status);
        Task<List<ProductCategoryModel>> GetAllTopAsync(int numberItem);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
        Task<object> GetMenus();
        Task<object> GetBreadcrumbById(int id);

    }
}
