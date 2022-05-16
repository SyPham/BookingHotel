using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.Data.Entities;

namespace BookingHotel.ApplicationRepository.Interface
{
    public interface IProductWishlistRepository : IBaseRepository<ProductWishlist>
    {
    }
}
