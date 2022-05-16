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
    public class ServiceCategoryService : IServiceCategoryService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IServiceCategoryRepository _serviceCatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ServiceCategoryService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IServiceCategoryRepository serviceCatRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _serviceCatRepository = serviceCatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceCategoryModel> FindById(int id)
        {
            var item = await _serviceCatRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
          
            return _mapper.Map<ServiceCategoryModel>(item);
        }

        public async Task<List<ServiceCategoryModel>> GetAllAsync()
        {
            var data = await _serviceCatRepository.FindAll().ProjectTo<ServiceCategoryModel>(_configMapper).ToListAsync(); 
            foreach (var item in data)
            {
                var parent = data.FirstOrDefault(x => x.Id == item.ParentId);
                if (parent != null)
                    item.ParentName = parent.Title;
            }
            return data;
        }

        public async Task<List<ServiceCategoryModel>> GetAllGroupAsync(int parentId)
        {
            return await _serviceCatRepository.FindAll(x => x.ParentId == parentId && x.Status == true).ProjectTo<ServiceCategoryModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ServiceCategoryModel>> GetByStatusAsync(bool status)
        {
            return await _serviceCatRepository.FindAll(x => x.Status == status).ProjectTo<ServiceCategoryModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _serviceCatRepository.FindAll().ProjectTo<ServiceCategoryModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(ServiceCategoryModel model)
        {
            var item = _mapper.Map<ServiceCategory>(model);
            try
            {
                _serviceCatRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(ServiceCategoryModel model)
        {
            var item = _mapper.Map<ServiceCategory>(model);
            try
            {
                _serviceCatRepository.Update(item);
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
            var item = _serviceCatRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _serviceCatRepository.Update(item);
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
            var item = _serviceCatRepository.FindById(id);
            try
            {
                _serviceCatRepository.Remove(item);
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
        public async Task<ProcessingResult> DeleteRangeAsync(List<int> ids)
        {
            var item = await _serviceCatRepository.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            try
            {
                _serviceCatRepository.RemoveMultiple(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { ids = ids });
            }
            return processingResult;
        }
    }
}
