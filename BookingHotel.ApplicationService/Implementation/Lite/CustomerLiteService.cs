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
using BookingHotel.Model.Lite;

namespace BookingHotel.ApplicationService.Implementation
{
    public class CustomerLiteService : ICustomerLiteService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public CustomerLiteService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(CustomerLiteModel model, int accountId)
        {
            var item = _mapper.Map<Customer>(model);
            try
            {
                var customer = await _customerRepository.FindAll().FirstOrDefaultAsync(x => x.Code == model.ReferralCode);
                if (customer != null) customer.Point += 3000;
                item.AccountId = accountId;
                item.CustomerTypeId = 1;
                item.Code = item.Phone;
                item.Point = 5000;
                item.CreateTime = DateTime.UtcNow;
                //
                _customerRepository.Add(item);
                if (customer != null)
                    _customerRepository.Update(customer);
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

        public async Task<ProcessingResult> UpdateAsync(CustomerLiteModel model, int accountId)
        {
            var item = await _customerRepository.FindAll().FirstOrDefaultAsync(x => x.AccountId == accountId);
            try
            {
                item.Email = model.Email;
                item.FullName = model.FullName;
                item.Address = model.Address;
                item.Gender = model.Gender;
                item.Birthday = model.Birthday;
                item.Avatar = model.Avatar;
                item.Phone = model.Phone;
                item.Tel = model.Tel;
                item.Address = model.Address;
                item.CompanyName = model.CompanyName;
                item.ModifyTime = DateTime.UtcNow;
                //
                _customerRepository.Update(item);
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
