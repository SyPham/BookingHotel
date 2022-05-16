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
    public interface IFeedbackService: IBaseService<FeedbackModel>
    {
        Task<List<FeedbackModel>> GetByStatusAsync(bool status);

        Task<List<FeedbackModel>> GetByStatusAndQtyAsync(bool status, int qty);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
    }
}
