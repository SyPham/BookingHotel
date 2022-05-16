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
    public interface IAssessService : IBaseService<AssessModel>
    {
        Task<Pager2> GetAllByKeyPaginationAsync(KeyPagination paramater, string keyName);
        Task<object> GetAllByKey(int keyId, string keyName);
        Task<decimal[]> GetMathByKeyAsync(int keyId, string keyName);
        Pager GetAllByAccountPagination(ParamaterPagination paramater, string keyName, int accountId);
    }
}
