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
    public class ProductWishlistService : IProductWishlistService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IProductWishlistRepository _productWishlistRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ProductWishlistService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IProductWishlistRepository productWishlistRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _productWishlistRepository = productWishlistRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ProductWishlistModel> FindById(int id)
        {
            return null;
        }

        public async Task<ProductWishlistModel> FindById(int accountId, int productId)
        {
            var item = await _productWishlistRepository.FindAll().FirstOrDefaultAsync(x => x.ProductId == productId && x.AccountId == accountId);
            return _mapper.Map<ProductWishlistModel>(item);
        }

        public async Task<List<ProductWishlistModel>> GetAllAsync()
        {
            return await _productWishlistRepository.FindAll().ProjectTo<ProductWishlistModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProductWishlistModel>> GetAllGroupAsync(int parentId)
        {
            return await _productWishlistRepository.FindAll().ProjectTo<ProductWishlistModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _productWishlistRepository.FindAll().ProjectTo<ProductWishlistModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(ProductWishlistModel model)
        {
            var item = _mapper.Map<ProductWishlist>(model);
            try
            {
                _productWishlistRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(ProductWishlistModel model)
        {
            var item = _mapper.Map<ProductWishlist>(model);
            try
            {
                _productWishlistRepository.Update(item);
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
            ProductWishlist item = new ProductWishlist();
            try
            {
                _productWishlistRepository.Remove(item);
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
            var item = _productWishlistRepository.FindAll().FirstOrDefault(x => x.AccountId == accountId && x.ProductId == productId);
            try
            {
                _productWishlistRepository.Remove(item);
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

        public async Task<ProcessingResult> DeleteByAccountAsync(int accountId)
        {
            var list = _productWishlistRepository.FindAll(x => x.AccountId == accountId);
            try
            {
                foreach (var item in list)
                {
                    _productWishlistRepository.Remove(item);
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

        public async Task<object> GetAllByAccountAsync(int accountId)
        {
             return await _productWishlistRepository.FindAll().ProjectTo<ProductWishlistModel>(_configMapper).ToListAsync();

        }
    }
}
