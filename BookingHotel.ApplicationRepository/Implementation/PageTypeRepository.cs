using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class PageTypeRepository : BaseRepository<PageType>, IPageTypeRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public PageTypeRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
