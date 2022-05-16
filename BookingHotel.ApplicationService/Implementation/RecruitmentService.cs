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
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public RecruitmentService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IRecruitmentRepository recruitmentRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _recruitmentRepository = recruitmentRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ProcessingResult> AddAsync(RecruitmentModel model)
        {
            var item = _mapper.Map<Recruitment>(model);
            try
            {
                _recruitmentRepository.Add(item);
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
            var item = _recruitmentRepository.FindById(id);
            try
            {
                _recruitmentRepository.Remove(item);
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

        public Task<RecruitmentModel> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RecruitmentModel>> GetAllAsync()
        {
            return await _recruitmentRepository.FindAll().ProjectTo<RecruitmentModel>(_configMapper).ToListAsync();
        }

        public async Task<List<RecruitmentModel>> GetByStatusAndQtyAsync(bool status, int qty)
        {
            return await _recruitmentRepository.FindAll(x => x.Status == status).Take(qty).ProjectTo<RecruitmentModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _recruitmentRepository.FindAll().ProjectTo<RecruitmentModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(RecruitmentModel model)
        {
            var item = _mapper.Map<Recruitment>(model);
            try
            {
                model.ModifyTime = DateTime.Now;
                _recruitmentRepository.Update(item);
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
            var item = _recruitmentRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _recruitmentRepository.Update(item);
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
