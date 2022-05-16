using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.Common;

namespace BookingHotel.ApplicationService.BaseServices
{
    public interface IBaseUploadService<T> where T : class
    {
        Task<ProcessingResult> AddWithUploadAsync(T model);
        Task<ProcessingResult> UpdateWithUploadAsync(T model);
        Task<ProcessingResult> DeleteWithUploadAsync(int id);
        Task<ProcessingResult> DeleteRangeWithUploadAsync(List<int> ids);
    }
}
