using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IFunctionService : IBaseService<FunctionModel>
    {
        Task<object> GetAllFunctionAsTree();
        Task<List<FunctionModel>> GetFunctionByModuleId(int moduleId);
        Task<object> GetMenus();
    }
}
