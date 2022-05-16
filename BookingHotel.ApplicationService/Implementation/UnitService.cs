using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class UnitService : IUnitService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public UnitService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IUnitRepository unitRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _unitRepository = unitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(UnitModel model)
        {
            var item = _mapper.Map<Unit>(model);
            try
            {
                _unitRepository.Add(item);
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
            var item = _unitRepository.FindById(id);
            try
            {
                _unitRepository.Remove(item);
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

        public async Task<UnitModel> FindById(int id)
        {
            var item = await _unitRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<UnitModel>(item);
        }
        
        public async Task<List<UnitModel>> GetAllAsync()
        {
            return await _unitRepository.FindAll().ProjectTo<UnitModel>(_configMapper).ToListAsync();
        }

        public async Task<List<UnitModel>> GetAllGroupAsync(int parentId)
        {
            return await _unitRepository.FindAll().ProjectTo<UnitModel>(_configMapper).ToListAsync();
        }

        public async Task<List<UnitModel>> GetByStatusAsync(bool status)
        {
            return await _unitRepository.FindAll(x => x.Status == status).ProjectTo<UnitModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _unitRepository.FindAll().ProjectTo<UnitModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(UnitModel model)
        {
            var item = _mapper.Map<Unit>(model);
            try
            {
                _unitRepository.Update(item);
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
            var item = _unitRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _unitRepository.Update(item);
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
