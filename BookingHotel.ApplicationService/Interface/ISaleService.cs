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
    public interface ISaleService : IBaseService<SaleModel>, IBaseUploadService<SaleModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<SaleModel>> GetAllGroupAsync(int parentId);
        Task<List<SaleModel>> GetByStatusAsync(bool status);
    }
}
