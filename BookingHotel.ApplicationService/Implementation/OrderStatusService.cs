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

namespace BookingHotel.ApplicationService.Implementation
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public OrderStatusService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IOrderStatusRepository orderStatusRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _orderStatusRepository = orderStatusRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderStatusModel> FindById(int id)
        {
            var item = await _orderStatusRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<OrderStatusModel>(item);
        }

        public async Task<List<OrderStatusModel>> GetAllAsync()
        {
            return await _orderStatusRepository.FindAll().ProjectTo<OrderStatusModel>(_configMapper).ToListAsync();
        }

        public async Task<List<OrderStatusModel>> GetAllGroupAsync(int parentId)
        {
            return await _orderStatusRepository.FindAll().ProjectTo<OrderStatusModel>(_configMapper).ToListAsync();
        }

        public async Task<List<OrderStatusModel>> GetByStatusAsync(bool status)
        {
            return await _orderStatusRepository.FindAll(x => x.Status == status).ProjectTo<OrderStatusModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _orderStatusRepository.FindAll().ProjectTo<OrderStatusModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(OrderStatusModel model)
        {
            var item = _mapper.Map<OrderStatus>(model);
            try
            {
                _orderStatusRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(OrderStatusModel model)
        {
            var item = _mapper.Map<OrderStatus>(model);
            try
            {
                _orderStatusRepository.Update(item);
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
            var item = _orderStatusRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _orderStatusRepository.Update(item);
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
            var item = _orderStatusRepository.FindById(id);
            try
            {
                _orderStatusRepository.Remove(item);
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
    }
}
