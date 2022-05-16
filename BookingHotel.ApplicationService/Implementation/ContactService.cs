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

namespace BookingHotel.ApplicationService.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ContactService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(ContactModel model)
        {
            var item = _mapper.Map<Contact>(model);
            try
            {
                _contactRepository.Add(item);
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
            var item = _contactRepository.FindById(id);
            try
            {
                _contactRepository.Remove(item);
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

        public async Task<ContactModel> FindById(int id)
        {
            return await _contactRepository.FindAll(x=> x.Id == id).AsNoTracking().ProjectTo<ContactModel>(_configMapper).FirstOrDefaultAsync();

        }

        public async Task<List<ContactModel>> GetAllAsync()
        {
            return await _contactRepository.FindAll().ProjectTo<ContactModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _contactRepository.FindAll().ProjectTo<ContactModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(ContactModel model)
        {
            var item = _mapper.Map<Contact>(model);
            try
            {
                _contactRepository.Update(item);
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

        public async Task<List<ContactModel>> GetByStatusAsync(bool status)
        {
            return await _contactRepository.FindAll(x => x.Status == status).ProjectTo<ContactModel>(_configMapper).ToListAsync();
        }

        public async Task<ProcessingResult> UpdateStatusAsync(int id)
        {
            var item = _contactRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _contactRepository.Update(item);
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
        public async Task<ProcessingResult> DeleteRangeAsync(List<int> ids)
        {

           
                try
                {
                    var items = await _contactRepository.FindAll(x => ids.Contains(x.Id)).ToListAsync();

                    _contactRepository.RemoveMultiple(items);
                    await _unitOfWork.SaveAll();
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = items };
                }
                catch (Exception ex)
                {
                    _logging.LogException(ex, new { id = ids });
                    return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                }
            return processingResult;
        }
    }
}
