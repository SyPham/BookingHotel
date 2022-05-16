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
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public CartService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<CartModel> FindById(int id)
        {
            return null;
        }

        public async Task<CartModel> FindById(int accountId, int productId)
        {
            var cart = await _cartRepository.FindAll().FirstOrDefaultAsync(x => x.AccountId == accountId && x.ProductId == productId);
            var product = _mapper.Map<ProductModel>(_productRepository.FindById(productId));
            
            var result = _mapper.Map<CartModel>(cart);
            result.Product = product;

            return result;
        }

        public async Task<List<CartModel>> GetAllAsync()
        {
            return await _cartRepository.FindAll().ProjectTo<CartModel>(_configMapper).ToListAsync();
        }

        public async Task<List<CartModel>> GetByAccountAsync(int accountId)
        {
            return await _cartRepository.FindAll(x => x.AccountId == accountId).ProjectTo<CartModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _cartRepository.FindAll().ProjectTo<CartModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(CartModel model)
        {
            var item = _mapper.Map<Cart>(model);
            try
            {
                _cartRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(CartModel model)
        {
            var item = _mapper.Map<Cart>(model);
            try
            {
                _cartRepository.Update(item);
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

        public async Task<ProcessingResult> UpdateQuantityAsync(int accountId, int productId, int qty)
        {
            var item = await _cartRepository.FindAll().FirstOrDefaultAsync(x => x.AccountId == accountId && x.ProductId == productId);
            try
            {
                item.Quantity = qty;
                //
                _cartRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { productId = productId });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = _cartRepository.FindById(id);
            try
            {
                _cartRepository.Remove(item);
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

        public async Task<ProcessingResult> DeleteAsync(int accountId, int productId)
        {
            var item = _cartRepository.FindAll().FirstOrDefault(x => x.AccountId == accountId && x.ProductId == productId);
            try
            {
                _cartRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { accountId = accountId, productId = productId });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteByAccount(int accountId)
        {
            var list = _cartRepository.FindAll(x => x.AccountId == accountId);
            try
            {
                foreach (var item in list)
                {
                    _cartRepository.Remove(item);
                }
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = list };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { accountId = accountId });
            }
            return processingResult;
        }

        public async Task<CartModel> AddItemintoCart(CartModel cartModel)
        {
            var data = await _productRepository.FindByIdAsync(cartModel.ProductId);

            var cartAdd = await _cartRepository.FindAll(x => x.AccountId == cartModel.AccountId && x.ProductId == cartModel.ProductId).AnyAsync();

            if (cartAdd == false)
            {
               
                var cart = new Cart
                {
                    AccountId = cartModel.AccountId,
                    ProductId = cartModel.ProductId,
                    Quantity = 1,
                    CreateTime = DateTime.Now
                };
                _cartRepository.Add(cart);
                await _cartRepository.SaveAll();

            }
            else
            {
                var cartUpdate = await _cartRepository.FindAll(x => x.AccountId == cartModel.AccountId && x.ProductId == cartModel.ProductId).FirstOrDefaultAsync();
                cartUpdate.Quantity += 1;
                _cartRepository.Update(cartUpdate);
                await _cartRepository.SaveAll();
            }
            return cartModel;
        }

        public async Task<List<CartModel>> GetCartItems(int accountId)
        {
            var cartModels = await _cartRepository.FindAll(b =>  b.AccountId == accountId)
                .Include(x=>x.Product)
                .ProjectTo<CartModel>(_configMapper).ToListAsync();
            return cartModels;
        }

        public async Task<List<CartModel>> ClearCart(int accountId)
        {
            var cartModels = await _cartRepository.FindAll(b => b.AccountId == accountId).ToListAsync();
            _cartRepository.RemoveMultiple(cartModels);
            await _cartRepository.SaveAll();
            return await GetCartItems(accountId);
        }

        public async Task<List<CartModel>> DeleteCartItem(int accountId, int productId)
        {
            var cart = await _cartRepository.FindAll(x => x.AccountId == accountId && x.ProductId == productId).FirstOrDefaultAsync();

            if (cart != null)
            {
                _cartRepository.Remove(cart);
                await _cartRepository.SaveAll();
            }

            return await GetCartItems(accountId);
        }

        public async Task<List<CartModel>> ChangeCartItemQuantity(int accountId, int productId)
        {
            var cartUpdate = await _cartRepository.FindAll(x => x.AccountId == accountId && x.ProductId == productId).FirstOrDefaultAsync();
            cartUpdate.Quantity += 1;
            _cartRepository.Update(cartUpdate);
            await _cartRepository.SaveAll();
            return await GetCartItems(accountId);
        }

        public async Task<ProcessingResult> DeleteByAccountAsync(int accountId)
        {
            var list = _cartRepository.FindAll(x => x.AccountId == accountId);
            try
            {
                foreach (var item in list)
                {
                    _cartRepository.Remove(item);
                }
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = list };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { accountId = accountId });
            }
            return processingResult;
        }

        public async Task<List<CartModel>> UpdateCartItemQuantity(int accountId, int productId, int quantity)
        {
               var cartUpdate = await _cartRepository.FindAll(x => x.AccountId == accountId && x.ProductId == productId).FirstOrDefaultAsync();
            cartUpdate.Quantity = quantity;
            _cartRepository.Update(cartUpdate);
            await _cartRepository.SaveAll();
            return await GetCartItems(accountId);
        }
    }
}
