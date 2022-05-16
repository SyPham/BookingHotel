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
    public interface IGroupFunctionService : IBaseService<GroupFunctionModel>
    {
        Task<object> GetFunctionsGroupFunction(int groupFunctionId);
        Task<List<GroupFunctionModel>> GetByStatusAsync(bool status);
        Task<ProcessingResult> PostFunctionGroupFunction(PostGroupFunctionRequest request);
        Task<ProcessingResult> PutFunctionGroupFunction(PutGroupFunctionRequest request);
    }
}
