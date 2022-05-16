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
    public class ProvinceService : IProvinceService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ProvinceService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IProvinceRepository provinceRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _provinceRepository = provinceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProvinceModel> FindById(int id)
        {
            var item = await _provinceRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ProvinceModel>(item);
        }

        public async Task<List<ProvinceModel>> GetAllAsync()
        {
            return await _provinceRepository.FindAll().ProjectTo<ProvinceModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProvinceModel>> GetAllGroupAsync(int parentId)
        {
            return await _provinceRepository.FindAll().ProjectTo<ProvinceModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProvinceModel>> GetByStatusAsync(bool status)
        {
            return await _provinceRepository.FindAll(x => x.Status == status).ProjectTo<ProvinceModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _provinceRepository.FindAll().ProjectTo<ProvinceModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(ProvinceModel model)
        {
            var item = _mapper.Map<Province>(model);
            try
            {
                _provinceRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(ProvinceModel model)
        {
            var item = _mapper.Map<Province>(model);
            try
            {
                _provinceRepository.Update(item);
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
            var item = _provinceRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _provinceRepository.Update(item);
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
            var item = _provinceRepository.FindById(id);
            try
            {
                _provinceRepository.Remove(item);
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
