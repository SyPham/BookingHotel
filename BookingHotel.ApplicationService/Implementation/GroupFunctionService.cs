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
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class GroupFunctionService : IGroupFunctionService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IGroupFunctionRepository _groupfunctionRepository;
        private readonly IBaseRepository<Function> _functionRepository;
        private readonly IBaseRepository<FunctionGroupFunction> _functionGroupfunctionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public GroupFunctionService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IGroupFunctionRepository groupfunctionRepository,
            IBaseRepository<Function> functionRepository,
            IBaseRepository<FunctionGroupFunction> functionGroupfunctionRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _groupfunctionRepository = groupfunctionRepository;
            _functionRepository = functionRepository;
            _functionGroupfunctionRepository = functionGroupfunctionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProcessingResult> AddAsync(GroupFunctionModel model)
        {
            var item = _mapper.Map<GroupFunction>(model);
            try
            {
                _groupfunctionRepository.Add(item);
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
            var item = _groupfunctionRepository.FindById(id);
            try
            {
                _groupfunctionRepository.Remove(item);
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

        public async Task<GroupFunctionModel> FindById(int id)
        {
            var item = await _groupfunctionRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<GroupFunctionModel>(item);
        }

        public async Task<List<GroupFunctionModel>> GetAllAsync()
        {
            return await _groupfunctionRepository.FindAll().ProjectTo<GroupFunctionModel>(_configMapper).ToListAsync();
        }

        public async Task<List<GroupFunctionModel>> GetByStatusAsync(bool status)
        {
            return await _groupfunctionRepository.FindAll(x => x.Status == status).ProjectTo<GroupFunctionModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _groupfunctionRepository.FindAll().ProjectTo<GroupFunctionModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> PostFunctionGroupFunction(PostGroupFunctionRequest request)
        {
            var model = _mapper.Map<GroupFunction>(request.GroupFunction);
            _groupfunctionRepository.Add(model);
            await _unitOfWork.SaveAll();
            var newFunctionGroupFunction = new List<FunctionGroupFunction>();
            foreach (var functionSystemId in request.FunctionIds)
            {
                newFunctionGroupFunction.Add(new FunctionGroupFunction(model.Id, functionSystemId));
            }
            var existingFunctionGroupFunction = _functionGroupfunctionRepository.FindAll().Where(x => x.GroupFunctionId == model.Id).ToList();
            _functionGroupfunctionRepository.RemoveMultiple(existingFunctionGroupFunction);
            _functionGroupfunctionRepository.AddRange(newFunctionGroupFunction.DistinctBy(x => new { x.GroupFunctionId, x.FunctionId }).ToList());
            try
            {
                var result = await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = request };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Save function package failed", Success = false };
                _logging.LogException(ex, request);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> PutFunctionGroupFunction(PutGroupFunctionRequest request)
        {
            var model = _mapper.Map<GroupFunction>(request.GroupFunction);

            _groupfunctionRepository.Update(model);
            await _unitOfWork.SaveAll();
            var newFunctionGroupFunction = new List<FunctionGroupFunction>();
            foreach (var functionSystemId in request.FunctionIds)
            {
                newFunctionGroupFunction.Add(new FunctionGroupFunction(model.Id, functionSystemId));
            }
            var existingFunctionGroupFunction = _functionGroupfunctionRepository.FindAll().Where(x => x.GroupFunctionId == model.Id).ToList();
            _functionGroupfunctionRepository.RemoveMultiple(existingFunctionGroupFunction);
            _functionGroupfunctionRepository.AddRange(newFunctionGroupFunction.DistinctBy(x => new { x.GroupFunctionId, x.FunctionId }).ToList());
            try
            {
                var result = await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = request };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Save group function failed", Success = false };
                _logging.LogException(ex, request);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(GroupFunctionModel model)
        {
            var item = _mapper.Map<GroupFunction>(model);
            try
            {
                model.ModifyTime = DateTime.Now;
                _groupfunctionRepository.Update(item);
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
        public async Task<object> GetFunctionsGroupFunction(int groupFunctionId)
        {

            if (groupFunctionId == 0)
            {
                var query = await _functionRepository.FindAll().Include(x => x.Module)
                                   .Select(a => new
                                   {
                                       a.Id,
                                       a.Title,
                                       Module = a.Module.Title,
                                       a.ParentId,
                                       IsChecked = false,

                                   }).ToListAsync();
                var groupBy = query.GroupBy(g => g.Module)
                    .Select(a => new
                    {
                        Module = a.Key,
                        Field = new
                        {
                            DataSource = a.Select(s => new
                            {
                                Id = s.Id,
                                Name = s.Title,
                                ParentId = s.ParentId,
                                s.IsChecked
                            }),
                            Id = "id",
                            Text = "name",
                            ParentId = "parentId",
                            hasChildren = "hasChild"
                        }
                    });
                return groupBy;
            }
            else
            {
                var query = await (from a in _functionRepository.FindAll().Include(x => x.Module)
                                   from fp in _functionGroupfunctionRepository.FindAll().Where(x => x.GroupFunctionId == groupFunctionId && a.Id == x.FunctionId).DefaultIfEmpty()
                                   select new
                                   {
                                       a.Id,
                                       a.Title,
                                       Module = a.Module.Title,
                                       a.ParentId,
                                       IsChecked = fp != null,
                                   }).ToListAsync();

                var groupBy = query.GroupBy(g => g.Module)
                         .Select(a => new
                         {
                             Module = a.Key,
                             Field = new
                             {
                                 DataSource = a.Select(s => new
                                 {
                                     Id = s.Id,
                                     Name = s.Title,
                                     ParentId = s.ParentId,
                                     s.IsChecked
                                 }),
                                 Id = "id",
                                 Text = "name",
                                 ParentId = "parentId",
                                 hasChildren = "hasChild"
                             }
                         });
                return groupBy;
            }



        }
    }
}
