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
using Microsoft.AspNetCore.Http;
using BookingHotel.Common.Helpers;

namespace BookingHotel.ApplicationService.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IProductWalletRepository _productWalletRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public OrderService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IOrderRepository orderRepository,
            IOrderStatusRepository orderStatusRepository,
            IProductWalletRepository productWalletRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
             IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _orderRepository = orderRepository;
            _orderStatusRepository = orderStatusRepository;
            _productWalletRepository = productWalletRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderModel> FindById(int id)
        {
            var item = await _orderRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<OrderModel>(item);
        }

        public async Task<List<OrderModel>> GetAllAsync()
        {
            var result = _orderRepository.FindAll().ProjectTo<OrderModel>(_configMapper);
            return await result.OrderByDescending(x => x.CreateTime).ToListAsync();
        }

        public async Task<List<OrderModel>> GetByOrderStatusAsync(int orderStatusId)
        {
            if (orderStatusId == 0) return await _orderRepository.FindAll( x => x.OrderStatusId > 0).ProjectTo<OrderModel>(_configMapper).OrderByDescending(x => x.CreateTime).ToListAsync();
            var result = _orderRepository.FindAll(x => x.OrderStatusId == orderStatusId).ProjectTo<OrderModel>(_configMapper);
            return await result.OrderByDescending(x => x.CreateTime).ToListAsync();
        }

        public async Task<List<OrderModel>> GetByPayTypeAsync(int payTypeId)
        {
            var result = _orderRepository.FindAll(x => x.PayTypeId == payTypeId).ProjectTo<OrderModel>(_configMapper);
            return await result.OrderByDescending(x => x.CreateTime).ToListAsync();
        }

        public async Task<List<OrderModel>> GetByCustomerAsync(int customerId)
        {
            var result = _orderRepository.FindAll(x => x.CustomerId == customerId).ProjectTo<OrderModel>(_configMapper);
            return await result.OrderByDescending(x => x.CreateTime).ToListAsync();
        }

        public async Task<Pager> GetByCustomerPaginationAsync(Filter2Pagination paramater, int customerId)
        {
            if (paramater.DateFrom != DateTime.MinValue && paramater.DateTo != DateTime.MinValue && paramater.typeId != 0)
            {
                var query = _orderRepository.FindAll(x => x.CustomerId == customerId
                                                  && x.PayTypeId == paramater.typeId
                                                  && x.CreateTime.Value.Date >= paramater.DateFrom.Date && x.CreateTime.Value.Date <= paramater.DateTo.Date
                                             ).ProjectTo<OrderModel>(_configMapper);
                return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
            else if (paramater.DateFrom != DateTime.MinValue && paramater.DateTo != DateTime.MinValue && paramater.typeId == 0)
            {
                var query = _orderRepository.FindAll(x => x.CustomerId == customerId
                                                  && x.CreateTime.Value.Date >= paramater.DateFrom.Date && x.CreateTime.Value.Date <= paramater.DateTo.Date
                                             ).ProjectTo<OrderModel>(_configMapper);
                return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
            else if ((paramater.DateFrom == DateTime.MinValue || paramater.DateTo == DateTime.MinValue) && paramater.typeId != 0)
            {
                var query = _orderRepository.FindAll(x => x.CustomerId == customerId
                                                  && x.PayTypeId == paramater.typeId
                                              ).ProjectTo<OrderModel>(_configMapper);
                return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
            else
            {
                var query = _orderRepository.FindAll(x => x.CustomerId == customerId).ProjectTo<OrderModel>(_configMapper);
                return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
        }

        public async Task<List<OrderModel>> GetByStatusAsync(int status)
        {
            if (status == 0) return await GetAllAsync();
            var result = _orderRepository.FindAll(x => x.Status == status).ProjectTo<OrderModel>(_configMapper);
            return await result.OrderByDescending(x => x.CreateTime).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _orderRepository.FindAll().ProjectTo<OrderModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(OrderModel model)
        {
            try
            {
                var item = _mapper.Map<Order>(model);
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

        public async Task<ProcessingResult> UpdateAsync(OrderModel model)
        {
            try
            {
                var item = _mapper.Map<Order>(model);
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

        public async Task<ProcessingResult> UpdateStatusAsync(int id, int status)
        {
            var item = _orderRepository.FindById(id);
            try
            {
                item.Status = status;
                _orderRepository.Update(item);
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
            var item = await _orderRepository.FindAll(x=> x.Id == id).Include(x=> x.OrderDetails).Include(x=> x.ProductWallets).FirstOrDefaultAsync();
            try
            {
                _orderDetailRepository.RemoveMultiple(item.OrderDetails.ToList());
                _productWalletRepository.RemoveMultiple(item.ProductWallets.ToList());
                _orderRepository.Remove(item);
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

        public async Task<ProcessingResult> SaveOrder(OrderModel orderModel)
        {
            try
            {
                // Lấy sản phẩm từ giỏ hàng
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
                var cartList = await _cartRepository.FindAll(x => x.AccountId == accountId).Include(x => x.Product).ToListAsync();
                // Tính tổng giá
                decimal totalPrice = 0;
                decimal totalSaleOff = 0;
                foreach (var item in cartList)
                {
                    var price = item.Quantity.Value * item.Product.Price.Value;
                    totalPrice += price;
                    // Nếu đang trong thời gian khuyến mãi thì mới tính
                    if (item.Product.IsSale)
                    {
                        // decimal price =  price * ((100 - item.Product.Sale.Value) / 100); // Tính % đã giảm giá
                        decimal percent = item.Quantity.Value * (item.Product.Sale.Value / 100); // Tính % giảm giá cho sản phẩm dựa theo số lượng: Ex: mua 2 cái Iphone giá 10trieu và giảm 20%. Tính % giảm = (2 * 20) / 100 = 0.4
                        decimal saleOff = price * percent; // Tính số tiền được giảm giá
                        totalSaleOff += saleOff;
                    }
                }
                // Tạo mới đơn hàng
                var model = _mapper.Map<Order>(orderModel);
                model.TotalPrice = totalPrice;
                model.SaleOff = totalSaleOff;
                foreach (var x in cartList)
                {
                    OrderDetail itemDetail = new OrderDetail();
                    itemDetail.OrderId = model.Id;
                    itemDetail.ProductId = x.ProductId;
                    itemDetail.Quantity = x.Quantity;
                    itemDetail.Price = (decimal?)x.Product.Price;
                    itemDetail.OriginalPrice = x.Product.OriginalPrice;
                    itemDetail.Option = model.Remark;
                    model.OrderDetails.Add(itemDetail);

                    ProductWallet wallet = new ProductWallet();
                    wallet.ProductId = x.ProductId;
                    wallet.AccountId = accountId;
                    wallet.Code = x.Product.Code;
                    wallet.MethodUsedId = x.Product.MethodUsedId;
                    wallet.CreateTime = DateTime.UtcNow;
                    model.ProductWallets.Add(wallet);
                }
                
                _orderRepository.Add(model);
                await _orderRepository.SaveAll();
                // Đặt hàng xong thì xóa giỏ hàng
                var cartRemove = await _cartRepository.FindAll(x => x.AccountId == accountId).ToListAsync();
                _cartRepository.RemoveMultiple(cartRemove);
                await _cartRepository.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = orderModel };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = 0 });
            }
            return processingResult;

        }

        public async Task<ProcessingResult> DeleteRangeAsync(List<int> ids)
        {

            foreach (var id in ids)
            {
                try
                {
                    var item = await _orderRepository.FindAll(x => x.Id == id).Include(x => x.OrderDetails).Include(x => x.ProductWallets).FirstOrDefaultAsync();
                    _orderDetailRepository.RemoveMultiple(item.OrderDetails.ToList());
                    _productWalletRepository.RemoveMultiple(item.ProductWallets.ToList());
                    _orderRepository.Remove(item);
                    await _unitOfWork.SaveAll();
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
                }
                catch (Exception ex)
                {
                    _logging.LogException(ex, new { id = id });
                    return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                }
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateOrderStatusAsync(int id, int orderStatusId)
        {
            var item = await _orderRepository.FindAll(x=>x.Id==id)
                .Include(x => x.OrderDetails)
                .Include(x=> x.PayType).FirstOrDefaultAsync();
            var orderStatusItem = _orderStatusRepository.FindById(orderStatusId);
            if (orderStatusItem == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "Không tìm thấy trạng thái đơn hàng này!", Success = true, Data = item };
            try
            {
                item.OrderStatusId = orderStatusId;
                _orderRepository.Update(item);
                await _unitOfWork.SaveAll();
                var itemResult = _mapper.Map<OrderModel>(item);

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = itemResult };
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
