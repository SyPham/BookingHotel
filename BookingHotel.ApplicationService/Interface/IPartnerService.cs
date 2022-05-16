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
    public interface IPartnerService : IBaseService<PartnerModel>, IBaseUploadService<PartnerModel>
    {
        Task<PartnerModel> FindByIdNoTracking(int id);
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<PartnerModel>> GetAllGroupAsync(int typeId);
        Task<List<PartnerModel>> GetAllByAccountAsync(int accountId);
        Task<List<PartnerModel>> GetByStatusAsync(bool status);
        Task<List<CustomerModel>> GetCustomersByPartnerId(int partnerId);
        Task<List<PartnerModel>> GetAllRelatedAsync(int id, int catID, int numberItem);
        Task<PartnerModel> GetByAccountAsync(int accountId);
        Task<bool> CheckExists(int id, string value, int type); // 1. Email, 2. Số điện thoại
Task<ProcessingResult> Update2(PartnerModel model);
        Task<object> GetAllProductByAccount(int accountId);
        Task<object> GetAllProductStopByAccount(int accountId);
        Task<object> GetAllProductIsSaleByAccount(int accountId);
        Task<ProcessingResult> UploadAvatar(UploadAvatarRequest request);
        PartnerModel GetByAccount(int accountId);


    }
}
