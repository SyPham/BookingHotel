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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public OrderDetailService(
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

        public async Task<OrderDetailModel> FindById(int id)
        {
            var item = await _orderDetailRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<OrderDetailModel>(item);
        }

        public async Task<OrderDetailModel> FindById(int orderId, int productId)
        {
            var item = await _orderDetailRepository.FindAll().FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId);
            return _mapper.Map<OrderDetailModel>(item);
        }

        public async Task<List<OrderDetailModel>> GetAllAsync()
        {
            return await _orderDetailRepository.FindAll().ProjectTo<OrderDetailModel>(_configMapper).ToListAsync();
        }

        public async Task<List<OrderDetailModel>> GetByOrderAsync(int orderId)
        {
            return await _orderDetailRepository.FindAll(x => x.OrderId == orderId).ProjectTo<OrderDetailModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _orderDetailRepository.FindAll().ProjectTo<OrderDetailModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(OrderDetailModel model)
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

        public async Task<ProcessingResult> UpdateAsync(OrderDetailModel model)
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

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = _orderDetailRepository.FindById(id);
            try
            {
                _orderDetailRepository.Remove(item);
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

        public async Task<ProcessingResult> DeleteByKeyAsync(int orderId, int productId)
        {
            var item = _orderDetailRepository.FindAll().FirstOrDefault(x => x.OrderId == orderId && x.ProductId == productId);
            try
            {
                _orderDetailRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { orderId = orderId, productId = productId });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteByOrderAsync(int orderId)
        {
            var list = _orderDetailRepository.FindAll(x => x.OrderId == orderId);
            try
            {
                foreach (var item in list)
                {
                    _orderDetailRepository.Remove(item);
                }
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = list };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { orderId = orderId });
            }
            return processingResult;
        }
    }
}
