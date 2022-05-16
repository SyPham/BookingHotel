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
    public class OrderLiteService : IOrderLiteService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public OrderLiteService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(OrderLiteModel model, int accountId)
        {
            try
            {
                var item = _mapper.Map<Order>(model);
                var customer = _customerRepository.FindAll().FirstOrDefault(x => x.AccountId == accountId);
                //
                item.CustomerId = customer.Id;
                item.OrderStatusId = 2;
                item.CreateTime = DateTime.UtcNow;

                foreach (var detail in item.OrderDetails)
                {
                    var product = _productRepository.FindById(detail.ProductId);

                    ProductWallet wallet = new ProductWallet();
                    wallet.ProductId = detail.ProductId;
                    wallet.AccountId = accountId;
                    wallet.Code = product != null ? product.Code : "";
                    wallet.MethodUsedId = product != null ? product.MethodUsedId : null;
                    wallet.CreateTime = DateTime.UtcNow;
                    //
                    item.ProductWallets.Add(wallet);
                }
                //
                _orderRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(OrderLiteModel model, int accountId)
        {
            try
            {
                var item = _mapper.Map<Order>(model);
                var customer = _customerRepository.FindAll().FirstOrDefaultAsync(x => x.AccountId == accountId);
                //
                item.CustomerId = customer.Id;
                item.ModifyTime = DateTime.UtcNow;
                //
                _orderRepository.Update(item);
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
