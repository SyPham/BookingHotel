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
    public class TabService : ITabService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly ITabRepository _tabRepository;
        private readonly IProductTabRepository _productTabRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public TabService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            ITabRepository tabRepository,
            IProductTabRepository productTabRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _tabRepository = tabRepository;
            _productTabRepository = productTabRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TabModel> FindById(int id)
        {
            var item = await _tabRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<TabModel>(item);
        }

        public async Task<List<TabModel>> GetAllAsync()
        {
            return await _tabRepository.FindAll().ProjectTo<TabModel>(_configMapper).ToListAsync();
        }

        public async Task<List<TabModel>> GetByProductAsync(int productId)
        {
            return await _tabRepository.GetAll().Join(_productTabRepository.GetAll().Where(x => x.ProductId == productId),
                tab => tab.Id,
                productTab => productTab.TabId,
                (tab, productTab) => tab).ProjectTo<TabModel>(_configMapper).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<TabModel>> GetAllGroupAsync(int parentId)
        {
            return await _tabRepository.FindAll().ProjectTo<TabModel>(_configMapper).ToListAsync();
        }

        public async Task<List<TabModel>> GetByStatusAsync(bool status)
        {
            return await _tabRepository.FindAll(x => x.Status == status).ProjectTo<TabModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _tabRepository.FindAll().ProjectTo<TabModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(TabModel model)
        {
            var item = _mapper.Map<Tab>(model);
            try
            {
                _tabRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(TabModel model)
        {
            var item = _mapper.Map<Tab>(model);
            try
            {
                _tabRepository.Update(item);
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
            var item = _tabRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _tabRepository.Update(item);
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
            var item = _tabRepository.FindById(id);
            try
            {
                _tabRepository.Remove(item);
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
