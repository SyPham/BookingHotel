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
    public interface IMethodUsedService : IBaseService<MethodUsedModel>
    {
        Task<List<MethodUsedModel>> GetByStatusAsync(bool status);

        Task<ProcessingResult> UpdateStatusAsync(int id);
    }
}
