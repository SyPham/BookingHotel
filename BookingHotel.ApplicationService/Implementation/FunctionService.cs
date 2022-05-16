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
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class FunctionService : IFunctionService
    {
        private readonly IBaseRepository<Function> _repoFunction;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private ProcessingResult processingResult;
        public FunctionService(
            IBaseRepository<Function> repoFunction,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            MapperConfiguration configMapper, ILogging logging)
        {
            _repoFunction = repoFunction;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configMapper = configMapper;
            _logging = logging;
        }
        public async Task<List<FunctionModel>> GetFunctionByModuleId(int moduleId)
        {
            return await _repoFunction.FindAll(x => x.ModuleId == moduleId && x.ParentId == null).ProjectTo<FunctionModel>(_configMapper).ToListAsync();
        }

        public async Task<ProcessingResult> AddAsync(FunctionModel model)
        {
            var item = _mapper.Map<Function>(model);
            try
            {
                _repoFunction.Add(item);
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
        private List<Function> FindChild(Function item)
        {
            var remove = new List<Function>();
            remove.Add(item);
            if (item.ParentId != null)
            {
                var list = _repoFunction.FindAll(x => x.ParentId == item.Id);
                foreach (var el in list)
                {
                    remove.AddRange(FindChild(el));
                }
            }
            return remove;
        }
        public async Task<ProcessingResult> DeleteAsync(string id)
        {
            var item = _repoFunction.FindById(id);
            try
            {
                if (item.ParentId != null)
                {

                    var list = FindChild(item);
                }

                _repoFunction.Remove(item);
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

        public async Task<FunctionModel> FindById(int id)
        {
            var item = await _repoFunction.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<FunctionModel>(item);
        }

        public async Task<List<FunctionModel>> GetAllAsync()
        {
            return await _repoFunction.FindAll().Include(x=> x.Module).ProjectTo<FunctionModel>(_configMapper).ToListAsync();
        }

        public async Task<object> GetAllFunctionAsTree()
        {
            var result = new List<FunctionModel>();
            var data = await _repoFunction.FindAll().Include(x=> x.Module).ProjectTo<FunctionModel>(_configMapper).ToListAsync();
            var lists = data.OrderBy(x => x.Title).AsHierarchy(x => x.Id, y => y.ParentId);
            return lists;
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _repoFunction.FindAll().ProjectTo<FunctionModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(FunctionModel model)
        {
            var item = _mapper.Map<Function>(model);
            try
            {
                _repoFunction.Update(item);
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

        public async Task<object> GetMenus()
        {
            var data = await _repoFunction.FindAll().Where(x => x.Action == "view")
         .Include(x => x.Module).ToListAsync();
            var res = data.AsHierarchy(x => x.Id, y => y.ParentId).ToList();
            //var res = data.Select(x => new TreeFunctionModel
            //{
            //    ModuleName = x.Module.Title,
            //    Sequence = x.Module.Position,
            //    Position = x.Position,
            //    Controller = x.Controller,
            //    Action = x.Action,
            //    Url = x.Url,
            //    Title = x.Title,
            //    Id = x.Id,
            //    ParentId = x.ParentId == null ? 0 : x.ParentId,
            //    Icon = x.Module.Icon
            //}).OrderBy(x => x.Sequence).GroupBy(x => new { x.ModuleName, x.Icon }).Select(x => new
            //{
            //    Name = x.Key.ModuleName,
            //    Icon = x.Key.Icon,
            //    HasChildrens = x.Any(),
            //    Childrens = x.OrderBy(x => x.Position).ToFlatToHierarchy()
            //}).ToList();
            return res;

        }

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = _repoFunction.FindById(id);
            try
            {
                if (item.ParentId != null)
                {

                    var list = FindChild(item);
                }

                _repoFunction.Remove(item);
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
