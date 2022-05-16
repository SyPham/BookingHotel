using AutoMapper;
using System;
using System.Collections.Generic;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;
#nullable disable

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class ServiceCategoryRepository : BaseRepository<ServiceCategory>, IServiceCategoryRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public ServiceCategoryRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
