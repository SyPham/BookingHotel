using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IRecruitmentService : IBaseService<RecruitmentModel>
    {
        Task<ProcessingResult> UpdateStatusAsync(int id);
        Task<List<RecruitmentModel>> GetByStatusAndQtyAsync(bool status, int qty);
    }
}
