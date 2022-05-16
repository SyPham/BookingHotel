using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.ApplicationService.Implementation
{
    public class OrderDetailLiteService : IOrderDetailLiteService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public OrderDetailLiteService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IOrderDetailRepository orderDetailRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ProcessingResult> AddAsync(OrderDetailLiteModel model, int accountId)
        {
            var item = _mapper.Map<OrderDetail>(model);
            try
            {
                _orderDetailRepository.Add(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(OrderDetailLiteModel model, int accountId)
        {
            var item = _mapper.Map<OrderDetail>(model);
            try
            {
                _orderDetailRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

    }
}
