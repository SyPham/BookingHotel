using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.ApplicationRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAll();
        void Save();
    }
}
