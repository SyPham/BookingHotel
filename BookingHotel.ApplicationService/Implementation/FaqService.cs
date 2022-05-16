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
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class FaqService : IFaqService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IFaqRepository _faqRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public FaqService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IFaqRepository faqRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _faqRepository = faqRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(FaqModel model)
        {
            var item = _mapper.Map<Faq>(model);
            try
            {
                _faqRepository.Add(item);
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
            var item = _faqRepository.FindById(id);
            try
            {
                _faqRepository.Remove(item);
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

        public async Task<FaqModel> FindById(int id)
        {
            var item = await _faqRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<FaqModel>(item);
        }

        public Task<FaqModel> FindByStatus(bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FaqModel>> GetAllAsync()
        {
            return await _faqRepository.FindAll().ProjectTo<FaqModel>(_configMapper).ToListAsync();
        }

        public async Task<List<FaqModel>> GetAllGroupAsync(int parentId)
        {
            return await _faqRepository.FindAll().ProjectTo<FaqModel>(_configMapper).ToListAsync();
        }

        public async Task<List<FaqModel>> GetByStatusAsync(bool status)
        {
            return await _faqRepository.FindAll(x => x.Status == status).ProjectTo<FaqModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _faqRepository.FindAll().ProjectTo<FaqModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(FaqModel model)
        {
            var item = _mapper.Map<Faq>(model);
            try
            {
                _faqRepository.Update(item);
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
            var item = _faqRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _faqRepository.Update(item);
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
