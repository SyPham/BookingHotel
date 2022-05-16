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

namespace BookingHotel.ApplicationService.Implementation
{
    public class ProductTabService : IProductTabService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IProductTabRepository _productTabRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ProductTabService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IProductTabRepository productTabRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _productTabRepository = productTabRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductTabModel> FindById(int id)
        {
            return new ProductTabModel();
        }

        public async Task<ProductTabModel> FindById(int productId, int tabId)
        {
            var item = await _productTabRepository.FindAll().FirstOrDefaultAsync(x => x.ProductId == productId && x.TabId == tabId);
            return _mapper.Map<ProductTabModel>(item);
        }

        public async Task<List<ProductTabModel>> GetAllAsync()
        {
            return await _productTabRepository.FindAll().ProjectTo<ProductTabModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProductTabModel>> GetAllGroupAsync(int parentId)
        {
            return await _productTabRepository.FindAll().ProjectTo<ProductTabModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _productTabRepository.FindAll().ProjectTo<ProductTabModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(ProductTabModel model)
        {
            var item = _mapper.Map<ProductTab>(model);
            try
            {
                _productTabRepository.Add(item);
                await _unitOfWork.SaveAll();
                //processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                //processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(ProductTabModel model)
        {
            var item = _mapper.Map<ProductTab>(model);
            try
            {
                _productTabRepository.Update(item);
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
            ProductTab item = new ProductTab();
            try
            {
                _productTabRepository.Remove(item);
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

        public async Task<ProcessingResult> DeleteAsync(int productId, int tabId)
        {
            var item = _productTabRepository.FindAll().FirstOrDefaultAsync(x => x.TabId == tabId && x.ProductId == productId);
            try
            {
                _productTabRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { productId = productId, tabId = tabId });
            }
            return processingResult;
        }
    }
}
