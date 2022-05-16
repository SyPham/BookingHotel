using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class TabTypeService : ITabTypeService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly ITabTypeRepository _tabtypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public TabTypeService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            ITabTypeRepository tabtypeRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _tabtypeRepository = tabtypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(TabTypeModel model)
        {
            var item = _mapper.Map<TabType>(model);
            try
            {
                _tabtypeRepository.Add(item);
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
            var item = _tabtypeRepository.FindById(id);
            try
            {
                _tabtypeRepository.Remove(item);
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

        public Task<TabTypeModel> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TabTypeModel>> GetAllAsync()
        {
            return await _tabtypeRepository.FindAll().ProjectTo<TabTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<List<TabTypeModel>> GetByStatusAsync(bool status)
        {
            return await _tabtypeRepository.FindAll(x => x.Status == status).ProjectTo<TabTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _tabtypeRepository.FindAll().ProjectTo<TabTypeModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(TabTypeModel model)
        {
            var item = _mapper.Map<TabType>(model);
            try
            {
                model.ModifyTime = DateTime.Now;
                _tabtypeRepository.Update(item);
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
    }
}
