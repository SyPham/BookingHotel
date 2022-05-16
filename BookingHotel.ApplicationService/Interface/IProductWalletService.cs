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
    public interface IProductWalletService : IBaseService<ProductWalletModel>
    {
        Task<Pager> GetByAccountPaginationAsync(ParamaterPagination paramater, int accountId);

        Task<ProductWalletModel> FindById(int accountId, int orderId,int productId);

        Task<ProcessingResult> DeleteAsync(int accountId, int orderId, int productId);

        Task<ProcessingResult> UpdateUseAsync(int accountId, int orderId, int productId);

        Task<ProcessingResult> UpdateAssessAsync(int accountId, int orderId, int productId);
     
        
    }
}
