using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public ContactRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
