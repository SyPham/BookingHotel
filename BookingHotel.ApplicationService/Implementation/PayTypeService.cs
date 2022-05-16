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
    public class PayTypeService : IPayTypeService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public PayTypeService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IPayTypeRepository paytypeRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _payTypeRepository = paytypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(PayTypeModel model)
        {
            var item = _mapper.Map<PayType>(model);
            try
            {
                _payTypeRepository.Add(item);
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
            var item = _payTypeRepository.FindById(id);
            try
            {
                _payTypeRepository.Remove(item);
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

        public async Task<PayTypeModel> FindById(int id)
        {
            var item = await _payTypeRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PayTypeModel>(item);
        }

        public Task<PayTypeModel> FindByStatus(bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PayTypeModel>> GetAllAsync()
        {
            return await _payTypeRepository.FindAll().ProjectTo<PayTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PayTypeModel>> GetAllGroupAsync(int parentId)
        {
            return await _payTypeRepository.FindAll().ProjectTo<PayTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PayTypeModel>> GetByStatusAsync(bool status)
        {
            return await _payTypeRepository.FindAll(x => x.Status == status).ProjectTo<PayTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _payTypeRepository.FindAll().ProjectTo<PayTypeModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(PayTypeModel model)
        {
            var item = _mapper.Map<PayType>(model);
            try
            {
                _payTypeRepository.Update(item);
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
            var item = _payTypeRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _payTypeRepository.Update(item);
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
