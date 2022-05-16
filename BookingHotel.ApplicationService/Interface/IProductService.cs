using System.Collections.Generic;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;
using X.PagedList;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IProductService : IBaseService<ProductModel>
    {
        Task<ProductModel> FindByIdNoTracking(int id);
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<object> LoadData(FilterRequest request);
        Task<List<ProductModel>> GetAllByCategoryAsync(int catId);
        Task<List<ProductModel>> GetAllBySaleAsync(int SaleId);
        Task<List<ProductModel>> GetAllByUntilAsync(int UntilId);
        Task<List<ProductModel>> GetAllByPartnerAsync(int partnerId);
        Task<List<ProductModel>> GetByStatusAsync(bool status);
        Task<ProductModel> IsWishlistFindById(int id, int accountId);
        Task<Pager> SearchPaginationAsync(SearchPagination paramater);
        Task<Pager> FilterPaginationAsync(FilterPagination paramater);
        Task<Pager> WishlistPaginationAsync(WishlistPagination paramater, int accountId);
        Task<object> GetWishlistAsync(int accountId);
        Task<List<ProductModel>> GetAllRelatedAsync(int id, int catID, int numberItem);
        Task<List<ProductModel>> GetAllNewAsync(int numberItem);
        Task<List<ProductModel>> GetAllHotAsync(int numberItem);
        Task<List<ProductModel>> GetAllRandomAsync(int numberItem);
        Task<List<ProductModel>> GetAllTopAsync(int numberItem);
        Task<List<ProductModel>> GetAllSaleAsync(int numberItem);
        Task<List<ProductModel>> GetAllBestSellerAsync(int numberItem);
        Task<ProcessingResult> UpdateViewAsync(int id);
        Task<ProcessingResult> UpdateValueAssessAsync(ValueAssessRequest request);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);

        Task<ProcessingResult> AddAsync(ProductActionModel model);

        Task<ProcessingResult> UpdateAsync(ProductActionModel model);

        Task<List<ProductCategoryHotModel>> GetSaleByCategoryAsync(int numberItem);

        Task<object> Search(int catId, string keyWord);
        Task<object> GetAllProductByAccount(int accountId);
        Task<object> GetAllProductIsUseByAccount(int accountId);
        Task<object> GetAllProductNotUseByAccount(int accountId);
        Task<List<ProductModel>> GetFeaturedProducts(int numberItem);

        Task<object> FilterNameAZ(int catId);
        Task<object> FilterNameZA(int catId);
        Task<object> FilterPriceAZ(int catId);
        Task<object> FilterPriceZA(int catId);
        Task<object> FilterRatingAZ(int catId);
        Task<object> FilterRatingZA(int catId);
    }
}
