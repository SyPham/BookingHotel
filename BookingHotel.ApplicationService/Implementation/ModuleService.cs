using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.BaseRepository;
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
    public class ModuleService : IModuleService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IBaseRepository<Function> _functionRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ModuleService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IBaseRepository<Function> functionRepository,
            IModuleRepository moduleRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _functionRepository = functionRepository;
            _moduleRepository = moduleRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProcessingResult> AddAsync(ModuleModel model)
        {
            var item = _mapper.Map<Module>(model);
            try
            {
                _moduleRepository.Add(item);
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
            var item = _moduleRepository.FindById(id);
            try
            {
                var f = await _functionRepository.FindAll(x => x.ModuleId == id).ToListAsync();


                _functionRepository.RemoveMultiple(f);
                await _unitOfWork.SaveAll();

                _moduleRepository.Remove(item);
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

        public async Task<ModuleModel> FindById(int id)
        {
            var item = await _moduleRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ModuleModel>(item);
        }

        public async Task<List<ModuleModel>> GetAllAsync()
        {
            return await _moduleRepository.FindAll().ProjectTo<ModuleModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ModuleModel>> GetByStatusAsync(bool status)
        {
            return await _moduleRepository.FindAll(x => x.Status == status).ProjectTo<ModuleModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _moduleRepository.FindAll().ProjectTo<ModuleModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(ModuleModel model)
        {
            var item = _mapper.Map<Module>(model);
            try
            {
                model.ModifyTime = DateTime.Now;
                _moduleRepository.Update(item);
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
