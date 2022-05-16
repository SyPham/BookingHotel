using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.Common;

namespace BookingHotel.ApplicationService.BaseServices
{
    public interface IBaseLiteService<T> where T : class
    {
        Task<ProcessingResult> AddAsync(T model, int accountId);
        Task<ProcessingResult> UpdateAsync(T model, int accountId);
    }
}
