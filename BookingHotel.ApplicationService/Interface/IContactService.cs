using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
   public interface IContactService: IBaseService<ContactModel>
    {
        Task<List<ContactModel>> GetByStatusAsync(bool status);
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
    }
}
