using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.Common;

namespace BookingHotel.ApplicationService.BaseServices
{
    public interface IBaseService<T> where T : class
    {
        Task<ProcessingResult> AddAsync(T model);
        Task<ProcessingResult> UpdateAsync(T model);
        Task<ProcessingResult> DeleteAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> FindById(int id);
        Task<Pager> PaginationAsync(ParamaterPagination paramater);
    }
}
