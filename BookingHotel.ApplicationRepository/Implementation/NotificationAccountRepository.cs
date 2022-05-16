using AutoMapper;
using System;
using System.Collections.Generic;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;
#nullable disable

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class NotificationAccountRepository : BaseRepository<NotificationAccount>, INotificationAccountRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public NotificationAccountRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
