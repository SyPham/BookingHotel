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
    public class PartnerLocalService : IPartnerLocalService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IPartnerLocalRepository _partnerLocalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public PartnerLocalService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IPartnerLocalRepository partnerLocalRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _partnerLocalRepository = partnerLocalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PartnerLocalModel> FindById(int id)
        {
            var item = await _partnerLocalRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PartnerLocalModel>(item);
        }

        public async Task<List<PartnerLocalModel>> GetAllAsync()
        {
            return await _partnerLocalRepository.FindAll().ProjectTo<PartnerLocalModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PartnerLocalModel>> GetAllGroupAsync(int partnerId)
        {
            return await _partnerLocalRepository.FindAll(x=>x.PartnerId == partnerId).ProjectTo<PartnerLocalModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PartnerLocalModel>> GetByStatusAsync(bool status)
        {
            return await _partnerLocalRepository.FindAll(x => x.Status == status).ProjectTo<PartnerLocalModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _partnerLocalRepository.FindAll().ProjectTo<PartnerLocalModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(PartnerLocalModel model)
        {
            var item = _mapper.Map<PartnerLocal>(model);
            try
            {
                _partnerLocalRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(PartnerLocalModel model)
        {
            var item = _mapper.Map<PartnerLocal>(model);
            try
            {
                _partnerLocalRepository.Update(item);
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
            var item = _partnerLocalRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _partnerLocalRepository.Update(item);
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
            var item = _partnerLocalRepository.FindById(id);
            try
            {
                _partnerLocalRepository.Remove(item);
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
