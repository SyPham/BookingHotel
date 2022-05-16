using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
