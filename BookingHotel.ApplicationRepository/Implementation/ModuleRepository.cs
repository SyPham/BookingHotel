using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Data.Entities;

namespace BookingHotel.ApplicationRepository.Implementation
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        private readonly VietCouponDBContext _context;
        private readonly IMapper _mapper;

        public ModuleRepository(VietCouponDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
