using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
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
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public NotificationService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            INotificationRepository notificationRepository,
           IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(NotificationModel model)
        {
            var item = _mapper.Map<Notification>(model);
            try
            {
                _notificationRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(NotificationModel model)
        {
            var item = _mapper.Map<Notification>(model);
            try
            {
                _notificationRepository.Update(item);
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

        public async Task<ProcessingResult> UpdateStatusAsync(int id)
        {
            var item = _notificationRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _notificationRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = id });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = _notificationRepository.FindById(id);
            try
            {
                _notificationRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = id });
            }
            return processingResult;
        }

        public async Task<NotificationModel> FindById(int id)
        {
            var item = await _notificationRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<NotificationModel>(item);
        }

        public async Task<List<NotificationModel>> GetAllAsync()
        {
            return await _notificationRepository.FindAll().ProjectTo<NotificationModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> GetByTypePaginationAsync(TypePagination paramater)
        {
            var query = _notificationRepository.FindAll(x => x.NotificationTypeId == paramater.typeId && x.Status == paramater.status).ProjectTo<NotificationModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<List<NotificationModel>> GetByStatusAsync(bool status)
        {
            return await _notificationRepository.FindAll(x => x.Status == status).OrderByDescending(x => x.CreateTime).ProjectTo<NotificationModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _notificationRepository.FindAll().ProjectTo<NotificationModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> DeleteRangeAsync(List<int> ids)
        {
            var item = await _notificationRepository.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            try
            {
                _notificationRepository.RemoveMultiple(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { ids = ids });
            }
            return processingResult;
        }
    }
}
