using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.ApplicationService.Implementation
{
    public class AccountLiteService : IAccountLiteService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly AppSettings _appSettings;
        private readonly ILogging _logging;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public AccountLiteService(
            IMapper mapper,
            MapperConfiguration configMapper,
            IOptions<AppSettings> appSettings,
            ILogging logging,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _appSettings = appSettings.Value;
            _logging = logging;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(AccountLiteModel model, int accountId)
        {
            var item = _mapper.Map<Account>(model);
            try
            {
                item.AccountTypeId = 2;
                item.AcceptTerms = true;
                item.Status = true;
                item.CreateTime = DateTime.UtcNow;
                //
                _accountRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(AccountLiteModel model, int accountId)
        {
            var item = await _accountRepository.FindAll().SingleOrDefaultAsync(x => x.Id == accountId);
            try
            {
                item.ModifyTime = DateTime.UtcNow;
                //
                _accountRepository.Update(item);
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
