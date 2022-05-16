using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
   public interface IAboutService : IBaseService<AboutModel>
    {
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
        Task<AboutModel> GetFisrtAsync();
    }
}
