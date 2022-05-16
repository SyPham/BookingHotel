using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IPageService : IBaseService<PageModel>
    {
        Task<List<PageModel>> GetByStatusAsync(bool status);
    }
}
