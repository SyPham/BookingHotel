using AutoMapper;
using System;
using System.Collections.Generic;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;
#nullable disable

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
