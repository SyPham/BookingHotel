using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class NotificationAccountLiteService : INotificationAccountLiteService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly INotificationAccountRepository _notificationAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public NotificationAccountLiteService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            INotificationAccountRepository notificationAccountRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _notificationAccountRepository = notificationAccountRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ProcessingResult> AddAsync(NotificationAccountLiteModel model, int accountId)
        {
            var item = _mapper.Map<NotificationAccount>(model);
            try
            {
                item.AccountId = accountId;
                item.CreateTime = DateTime.UtcNow;
                //
                _notificationAccountRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(NotificationAccountLiteModel model, int accountId)
        {
            var item = _mapper.Map<NotificationAccount>(model);
            try
            {
                item.AccountId = accountId;
                //
                _notificationAccountRepository.Update(item);
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
