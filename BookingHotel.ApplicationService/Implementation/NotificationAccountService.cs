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
using System.Linq;
using Microsoft.AspNetCore.Http;
using BookingHotel.Common.Helpers;

namespace BookingHotel.ApplicationService.Implementation
{
    public class NotificationAccountService : INotificationAccountService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationAccountRepository _notificationAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public NotificationAccountService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IHttpContextAccessor httpContextAccessor,
            INotificationAccountRepository notificationAccountRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _httpContextAccessor = httpContextAccessor;
            _notificationAccountRepository = notificationAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationAccountModel> FindById(int id)
        {
            return null;
        }

        public async Task<NotificationAccountModel> FindById(int accountId, int notificationId)
        {
            var item = await _notificationAccountRepository.FindAll().FirstOrDefaultAsync(x => x.NotificationId == notificationId && x.AccountId == accountId);
            return _mapper.Map<NotificationAccountModel>(item);
        }

        public async Task<List<NotificationAccountModel>> GetAllAsync()
        {
            return await _notificationAccountRepository.FindAll().ProjectTo<NotificationAccountModel>(_configMapper).ToListAsync();
        }

        public async Task<List<NotificationAccountModel>> GetAllGroupAsync(int parentId)
        {
            return await _notificationAccountRepository.FindAll().ProjectTo<NotificationAccountModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _notificationAccountRepository.FindAll().ProjectTo<NotificationAccountModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(NotificationAccountModel model)
        {
            var item = _mapper.Map<NotificationAccount>(model);
            try
            {
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

        public async Task<ProcessingResult> UpdateAsync(NotificationAccountModel model)
        {
            var item = _mapper.Map<NotificationAccount>(model);
            try
            {
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

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            NotificationAccount item = new NotificationAccount();
            try
            {
                _notificationAccountRepository.Remove(item);
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

        public async Task<ProcessingResult> DeleteAsync(int accountId, int notificationId)
        {
            var item = _notificationAccountRepository.FindAll().FirstOrDefaultAsync(x => x.AccountId == accountId && x.NotificationId == notificationId);
            try
            {
                _notificationAccountRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { notificationId = notificationId, accountId = accountId });
            }
            return processingResult;
        }

        public async Task<List<NotificationModel>> GetNotificationByAccount(bool status)
        {
             var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            //
            var list = await _notificationAccountRepository.FindAll(x => x.AccountId == accountId)
                .Include(x=>x.Account)
                .Include(x=>x.Notification)
                     .ThenInclude(x=>x.NotificationType)
                .Select(x=>x.Notification)
                .Where(x=>x.Status == status)
                .ProjectTo<NotificationModel>(_configMapper)
                .ToListAsync();
            return list;
        }
    }
}
