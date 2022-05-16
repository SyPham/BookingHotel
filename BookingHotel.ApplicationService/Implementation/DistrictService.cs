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
    public class DistrictService : IDistrictService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IDistrictRepository _districtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public DistrictService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IDistrictRepository districtRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _districtRepository = districtRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DistrictModel> FindById(int id)
        {
            var item = await _districtRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<DistrictModel>(item);
        }

        public async Task<List<DistrictModel>> GetAllAsync()
        {
            return await _districtRepository.FindAll().ProjectTo<DistrictModel>(_configMapper).ToListAsync();
        }

        public async Task<List<DistrictModel>> GetAllGroupAsync(int provinceId)
        {
            return await _districtRepository.FindAll(x=>x.ProvinceId == provinceId).ProjectTo<DistrictModel>(_configMapper).ToListAsync();
        }

        public async Task<List<DistrictModel>> GetByStatusAsync(bool status)
        {
            return await _districtRepository.FindAll(x => x.Status == status).ProjectTo<DistrictModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _districtRepository.FindAll().ProjectTo<DistrictModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(DistrictModel model)
        {
            var item = _mapper.Map<District>(model);
            try
            {
                _districtRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(DistrictModel model)
        {
            var item = _mapper.Map<District>(model);
            try
            {
                _districtRepository.Update(item);
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
            var item = _districtRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _districtRepository.Update(item);
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
            var item = _districtRepository.FindById(id);
            try
            {
                _districtRepository.Remove(item);
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
