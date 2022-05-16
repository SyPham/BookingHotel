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
    public class PageTypeService : IPageTypeService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IPageTypeRepository _pagetypekRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public PageTypeService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IPageTypeRepository pagetypeRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _pagetypekRepository = pagetypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(PageTypeModel model)
        {
            var item = _mapper.Map<PageType>(model);
            try
            {
                _pagetypekRepository.Add(item);
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
            var item = _pagetypekRepository.FindById(id);
            try
            {
                _pagetypekRepository.Remove(item);
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

        public Task<PageTypeModel> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PageTypeModel>> GetAllAsync()
        {
            return await _pagetypekRepository.FindAll().ProjectTo<PageTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PageTypeModel>> GetByStatusAsync(bool status)
        {
            return await _pagetypekRepository.FindAll(x => x.Status == status).ProjectTo<PageTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _pagetypekRepository.FindAll().ProjectTo<PageTypeModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(PageTypeModel model)
        {
            var item = _mapper.Map<PageType>(model);
            try
            {
                model.ModifyTime = DateTime.Now;
                _pagetypekRepository.Update(item);
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
