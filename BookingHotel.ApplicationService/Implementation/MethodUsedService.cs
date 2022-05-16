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
    public class MethodUsedService : IMethodUsedService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IMethodUsedRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public MethodUsedService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IMethodUsedRepository unitRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _unitRepository = unitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MethodUsedModel> FindById(int id)
        {
            var item = await _unitRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<MethodUsedModel>(item);
        }

        public async Task<List<MethodUsedModel>> GetAllAsync()
        {
            return await _unitRepository.FindAll().ProjectTo<MethodUsedModel>(_configMapper).ToListAsync();
        }

        public async Task<List<MethodUsedModel>> GetByStatusAsync(bool status)
        {
            return await _unitRepository.FindAll(x => x.Status == status).ProjectTo<MethodUsedModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _unitRepository.FindAll().ProjectTo<MethodUsedModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(MethodUsedModel model)
        {
            try
            {
                var item = _mapper.Map<MethodUsed>(model);
                //
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

        public async Task<ProcessingResult> UpdateAsync(MethodUsedModel model)
        {
            
            try
            {
                var item = _mapper.Map<MethodUsed>(model);
                //
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
            try
            {
                var item = _unitRepository.FindById(id);
                //
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

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            try
            {
                var item = _unitRepository.FindById(id);
                //
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
    }
}
