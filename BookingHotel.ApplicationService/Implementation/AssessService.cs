using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AssessService : IAssessService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IAssessRepository _assessRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public AssessService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IAssessRepository assessRepository,
            IAccountRepository accountRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _assessRepository = assessRepository;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AssessModel> FindById(int id)
        {
            var item = await _assessRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<AssessModel>(item);
        }

        public async Task<List<AssessModel>> GetAllAsync()
        {
            return await _assessRepository.FindAll().ProjectTo<AssessModel>(_configMapper).ToListAsync();
        }

        public async Task<decimal[]> GetMathByKeyAsync(int keyId, string keyName)
        {
            decimal avg = 0;
            decimal sum = 0;
            decimal count = 0;
            //
            List<Assess> assessList = await _assessRepository.FindAll(x => x.KeyId == keyId && x.KeyName == keyName).ToListAsync();
            if (assessList != null && assessList.Count > 0)
            {
                count = assessList.Count;
                sum = assessList.Sum(x => x.NumberStar).Value;
                avg = Math.Round(sum / count, 1);
            }
            return new decimal[] { avg, count }; ;
        }

        public Pager GetAllByAccountPagination(ParamaterPagination paramater, string keyName, int accountId)
        {
            var query = _assessRepository.FindAll(x => x.AccountId == accountId && x.KeyName == keyName).ProjectTo<AssessModel>(_configMapper);

            List<AssessModel> result = new List<AssessModel>();
            foreach (var item in query)
            {
                AssessModel model = new AssessModel();
                model.Id = item.Id;
                model.FullName = item.FullName;
                model.Content = item.Content;
                model.Note = item.Note;
                model.NumberStar = item.NumberStar;
                model.KeyId = item.KeyId;
                model.KeyName = item.KeyName;
                model.CreateTime = item.CreateTime;
                model.CreateBy = item.CreateBy;
                model.ModifyBy = item.ModifyBy;
                model.ModifyTime = item.ModifyTime;
                model.AccountId = item.AccountId;
                result.Add(model);
            }
            if (keyName == "Product")
            {
                foreach (var item in result)
                {
                    var product = _productRepository.FindById(item.KeyId);
                    item.ProductRequest = _mapper.Map<ProductModel>(product);
                }
            }

            return result.AsQueryable().OrderByDescending(x => x.CreateTime).ToPagination(paramater.page, paramater.pageSize);
        }

        public async Task<Pager2> GetAllByKeyPaginationAsync(KeyPagination paramater, string keyName)
        {
            var groupResult = new object();
            if (keyName == "Product")
                groupResult = _productRepository.FindById(paramater.id);
            var query = _assessRepository.FindAll(x => x.KeyId == paramater.id && x.KeyName == keyName).ProjectTo<AssessModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).ToPagination2Async(groupResult, paramater.page, paramater.pageSize);
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _assessRepository.FindAll().ProjectTo<AssessModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(AssessModel model)
        {
            try
            {
                var item = _mapper.Map<Assess>(model);
                item.CreateTime = DateTime.UtcNow;
                //
                _assessRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(AssessModel model)
        {
            try
            {
                var item = _mapper.Map<Assess>(model);
                item.ModifyTime = DateTime.UtcNow;
                //
                _assessRepository.Update(item);
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
            var item = _assessRepository.FindById(id);
            try
            {
                _assessRepository.Remove(item);
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

        public async Task<object> GetAllByKey(int keyId, string keyName)
        {
            var data = await _assessRepository.FindAll(x=>x.KeyId == keyId && x.KeyName == keyName).Include(x=>x.Account).ProjectTo<AssessWebModel>(_configMapper).ToListAsync();
            foreach (var item in data)
            {
                var account  = await _accountRepository.FindAll(x=>x.Id == item.AccountId).Include(x=>x.Customers).Include(x=>x.Partners).Select(x=> new {x.Customers, x.Partners}).FirstOrDefaultAsync();
                item.CustomerAvatar = account.Customers.Any() ? account.Customers.First().Avatar : "";
            }  
            return data;
        }
    }
}
