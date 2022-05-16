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
    public class ProductWalletService : IProductWalletService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IProductWalletRepository _productWalletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ProductWalletService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IProductWalletRepository productWalletRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _productWalletRepository = productWalletRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ProductWalletModel> FindById(int id)
        {
            return null;
        }

        public async Task<ProductWalletModel> FindById(int accountId, int orderId, int productId)
        {
            var item = await _productWalletRepository.FindAll().FirstOrDefaultAsync(x => x.ProductId == productId && x.OrderId == orderId && x.AccountId == accountId);
            return _mapper.Map<ProductWalletModel>(item);
        }

        public async Task<List<ProductWalletModel>> GetAllAsync()
        {
            return await _productWalletRepository.FindAll().ProjectTo<ProductWalletModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> GetByAccountPaginationAsync(ParamaterPagination paramater, int accountId)
        {
            var query = _productWalletRepository.FindAll(x => x.AccountId == accountId).ProjectTo<ProductWalletModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _productWalletRepository.FindAll().ProjectTo<ProductWalletModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(ProductWalletModel model)
        {
            var item = _mapper.Map<ProductWallet>(model);
            try
            {
                _productWalletRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(ProductWalletModel model)
        {
            var item = _mapper.Map<ProductWallet>(model);
            try
            {
                _productWalletRepository.Update(item);
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

        public async Task<ProcessingResult> UpdateUseAsync(int accountId, int orderId, int productId)
        {
            try
            {
                var item = _productWalletRepository.FindAll().FirstOrDefault(x => x.ProductId == productId && x.OrderId == orderId && x.AccountId == accountId);
                //
                item.IsUse = item.IsUse == true ? false : true;
                item.UseTime = DateTime.UtcNow;
                //
                _productWalletRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { productId = productId, accountId = accountId });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAssessAsync(int accountId, int orderId, int productId)
        {
            try
            {
                var item = _productWalletRepository.FindAll().FirstOrDefault(x => x.ProductId == productId && x.OrderId == orderId && x.AccountId == accountId);

                item.IsAssess = item.IsAssess == true ? false : true;
                _productWalletRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { productId = productId, accountId = accountId });
            }
            return processingResult;
        }

        public Task<ProcessingResult> DeleteAsync(int id)
        {
            return null;
        }

        public async Task<ProcessingResult> DeleteAsync(int accountId, int orderId, int productId)
        {
            try
            {
                var item = _productWalletRepository.FindAll().FirstOrDefault(x => x.ProductId == productId && x.OrderId == orderId && x.AccountId == accountId);
                //
                _productWalletRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { productId = productId, accountId = accountId });
            }
            return processingResult;
        }

        public Task<object> GetAllProductByAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAllProductIsUseByAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAllProductNotUseByAccount(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
