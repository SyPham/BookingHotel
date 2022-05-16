using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly INotificationTypeRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public NotificationTypeService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            INotificationTypeRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(NotificationTypeModel model)
        {
            var item = _mapper.Map<NotificationType>(model);
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

        public async Task<ProcessingResult> UpdateAsync(NotificationTypeModel model)
        {
            var item = _mapper.Map<NotificationType>(model);
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

        public async Task<List<NotificationTypeModel>> GetByStatusAsync(bool status)
        {
            return await _notificationRepository.FindAll(x => x.Status == status).ProjectTo<NotificationTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<NotificationTypeModel> FindById(int id)
        {
            var item = await _notificationRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<NotificationTypeModel>(item);
        }

        public async Task<List<NotificationTypeModel>> GetAllAsync()
        {
            return await _notificationRepository.FindAll().ProjectTo<NotificationTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _notificationRepository.FindAll().ProjectTo<NotificationTypeModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

    }
}
