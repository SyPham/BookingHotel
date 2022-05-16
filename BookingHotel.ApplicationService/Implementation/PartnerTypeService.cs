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
    public class PartnerTypeService : IPartnerTypeService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IPartnerTypeRepository _partnerTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public PartnerTypeService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IPartnerTypeRepository bannerTypeRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _partnerTypeRepository = bannerTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PartnerTypeModel> FindById(int id)
        {
            var item = await _partnerTypeRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PartnerTypeModel>(item);
        }

        public async Task<List<PartnerTypeModel>> GetAllAsync()
        {
            return await _partnerTypeRepository.FindAll().ProjectTo<PartnerTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PartnerTypeModel>> GetByStatusAsync(bool status)
        {
            return await _partnerTypeRepository.FindAll(x => x.Status == status).ProjectTo<PartnerTypeModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _partnerTypeRepository.FindAll().ProjectTo<PartnerTypeModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(PartnerTypeModel model)
        {
            var item = _mapper.Map<PartnerType>(model);
            try
            {
                _partnerTypeRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(PartnerTypeModel model)
        {
            var item = _mapper.Map<PartnerType>(model);
            try
            {
                _partnerTypeRepository.Update(item);
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
            var item = _partnerTypeRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _partnerTypeRepository.Update(item);
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
            var item = _partnerTypeRepository.FindById(id);
            try
            {
                _partnerTypeRepository.Remove(item);
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
