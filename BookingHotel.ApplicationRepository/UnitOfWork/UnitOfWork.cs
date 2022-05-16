using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.Data.Entities;
//using BookingHotel.Data.Entities;

namespace BookingHotel.ApplicationRepository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VietCouponDBContext _context;

        public UnitOfWork(VietCouponDBContext context)
        {
            _context = context;
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
