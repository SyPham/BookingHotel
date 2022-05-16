using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class PartnerService : IPartnerService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IBaseRepository<CustomerPartner> _customerPartnerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public PartnerService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IHostingEnvironment currentEnvironment,
            IPartnerRepository partnerRepository,
            IHttpContextAccessor httpContextAccessor,
            IProductRepository productRepository,
            IAccountRepository accountRepository,
            IBaseRepository<CustomerPartner> customerPartnerRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
            _partnerRepository = partnerRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _customerPartnerRepository = customerPartnerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PartnerModel> FindById(int id)
        {
            var item = await _partnerRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PartnerModel>(item);
        }

        public async Task<PartnerModel> FindByIdNoTracking(int id)
        {
            var item = await _partnerRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PartnerModel>(item);
        }
        public PartnerModel GetByAccount(int accountId)
        {
            var item = _partnerRepository.FindAll().FirstOrDefault(x => x.AccountId == accountId);
            return _mapper.Map<PartnerModel>(item);
        }
        public async Task<List<PartnerModel>> GetAllAsync()
        {
            return await _partnerRepository.FindAll().Include(x => x.PartnerType).ProjectTo<PartnerModel>(_configMapper).ToListAsync();
        }

        public async Task<List<PartnerModel>> GetAllGroupAsync(int typeId)
        {
            return await _partnerRepository.FindAll(x => x.PartnerTypeId == typeId).ProjectTo<PartnerModel>(_configMapper).ToListAsync();
        }
        public async Task<List<PartnerModel>> GetAllRelatedAsync(int id, int catId, int numberItem)
        {
            var query = _partnerRepository.FindAll(x => x.PartnerTypeId == catId && x.Id != id && x.Status == true).Include(x => x.PartnerLocals).ProjectTo<PartnerModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }

        public async Task<List<PartnerModel>> GetAllByAccountAsync(int accountId)
        {
            return await _partnerRepository.FindAll(x => x.AccountId == accountId).ProjectTo<PartnerModel>(_configMapper).ToListAsync();
        }
        public async Task<PartnerModel> GetByAccountAsync(int accountId)
        {
            var item = await _partnerRepository.FindAll(x => x.AccountId == accountId).ProjectTo<PartnerModel>(_configMapper).FirstOrDefaultAsync(x => x.AccountId == accountId);
            var account = await _accountRepository.FindAll(x => x.Id == accountId).ProjectTo<AccountModel>(_configMapper).FirstOrDefaultAsync();
            account.Password = "";
            item.Account = account;
            return item;
        }
        public async Task<ProcessingResult> Update2(PartnerModel model)
        {
            var item = await _partnerRepository.FindAll().FirstOrDefaultAsync(x => x.Id == model.Id);
            try
            {
                item.Title = model.Title;
                item.Email = model.Email;
                item.Representative = model.Representative;
                item.Phone = model.Phone;
                item.Description = model.Description;
                item.Content = model.Content;
                item.ModifyTime = DateTime.UtcNow;
                _partnerRepository.Update(item);
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
        public async Task<bool> CheckExists(int id, string value, int type) //1.Email, 2.Số điện thoại
        {
            if (string.IsNullOrEmpty(value)) return true;
            bool result = false;
            value = value.Trim();
            if (type == 1)
            {
                var item = await _partnerRepository.FindAll().FirstOrDefaultAsync(x => x.Email.Equals(value) && x.Id != id);
                if (item != null)
                    result = true;
            }
            else if (type == 2)
            {
                var item = await _partnerRepository.FindAll().FirstOrDefaultAsync(x => x.Phone.Equals(value) && x.Id != id);
                if (item != null)
                    result = true;
            }
            return result;
        }
        public async Task<List<PartnerModel>> GetByStatusAsync(bool status)
        {
            return await _partnerRepository.FindAll(x => x.Status == status).ProjectTo<PartnerModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _partnerRepository.FindAll().ProjectTo<PartnerModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(PartnerModel model)
        {
            var item = _mapper.Map<Partner>(model);
            try
            {
                _partnerRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(PartnerModel model)
        {
            var item = _mapper.Map<Partner>(model);
            try
            {
                _partnerRepository.Update(item);
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
            var item = _partnerRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _partnerRepository.Update(item);
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
            var item = _partnerRepository.FindById(id);
            try
            {
                _partnerRepository.Remove(item);
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

        public async Task<ProcessingResult> AddWithUploadAsync(PartnerModel model)
        {
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            IFormFile filesBanner = model.FilesThumb.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\partner\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\partner\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            var bannerUniqueFileName = string.Empty;
            var bannerFolderPath = "FileUploads\\images\\partner\\banner";
            string uploadBannerFolder = Path.Combine(_currentEnvironment.WebRootPath, bannerFolderPath);

            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            fileExtension.CreateFolder(uploadThumbFolder);
            fileExtension.CreateFolder(uploadBannerFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/partner/avatar/{avatarUniqueFileName}";
            }

            if (!filesThumb.IsNullOrEmpty())
            {
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                model.Thumb = $"/FileUploads/images/partner/thumb/{thumbUniqueFileName}";
            }
            if (!filesBanner.IsNullOrEmpty())
            {
                bannerUniqueFileName = await fileExtension.WriteAsync(filesBanner, $"{uploadBannerFolder}\\{bannerUniqueFileName}");
                model.Banner = $"/FileUploads/images/partner/banner/{bannerUniqueFileName}";
            }
            var item = _mapper.Map<Partner>(model);
            try
            {
                _partnerRepository.Add(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                if (!thumbUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadThumbFolder}\\{thumbUniqueFileName}");

                if (!bannerUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadBannerFolder}\\{bannerUniqueFileName}");

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateWithUploadAsync(PartnerModel model)
        {
            // string token = Request.Headers["Authorization"];
            // var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (await FindByIdNoTracking(model.Id) == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            var item = _mapper.Map<Partner>(model);

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\partner\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\partner\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            var bannerUniqueFileName = string.Empty;
            var bannerFolderPath = "FileUploads\\images\\partner\\banner";
            string uploadBannerFolder = Path.Combine(_currentEnvironment.WebRootPath, bannerFolderPath);


            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            IFormFile filesBanner = model.FilesBanner.FirstOrDefault();

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/partner/avatar/{avatarUniqueFileName}";
            }

            if (filesThumb != null)
            {
                if (!item.Thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Thumb.Replace("/", "\\").Replace("/", "\\")}");
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                item.Thumb = $"/FileUploads/images/partner/thumb/{thumbUniqueFileName}";
            }
            if (filesBanner != null)
            {
                if (!item.Banner.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Banner.Replace("/", "\\").Replace("/", "\\")}");
                bannerUniqueFileName = await fileExtension.WriteAsync(filesBanner, $"{uploadBannerFolder}\\{bannerUniqueFileName}");
                item.Banner = $"/FileUploads/images/partner/banner/{bannerUniqueFileName}";
            }

            try
            {
                _partnerRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                // Nếu tạo ra file rồi mã lưu db bị lỗi thì xóa file vừa tạo đi
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                if (!thumbUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadThumbFolder}\\{thumbUniqueFileName}");

                if (!bannerUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadBannerFolder}\\{bannerUniqueFileName}");

                // Không thêm được thì xóa file vừa tạo đi
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteWithUploadAsync(int id)
        {
            var item = _partnerRepository.FindById(id);
            var avatar = item.Avatar;
            var thumb = item.Thumb;
            var banner = item.Banner;
            try
            {
                _partnerRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                FileExtension fileExtension = new FileExtension();
                if (!avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");

                if (!thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{thumb.Replace("/", "\\").Replace("/", "\\")}");
                if (!banner.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{banner.Replace("/", "\\").Replace("/", "\\")}");

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = id });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteRangeWithUploadAsync(List<int> ids)
        {
            FileExtension fileExtension = new FileExtension();

            var query = await _partnerRepository.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var item in query)
            {
                try
                {
                    var avatar = item.Avatar;
                    var thumb = item.Thumb;
                    var banner = item.Banner;
                    _partnerRepository.Remove(item);
                    await _unitOfWork.SaveAll();

                    if (!avatar.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");

                    if (!thumb.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{thumb.Replace("/", "\\").Replace("/", "\\")}");

                    if (!banner.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{banner.Replace("/", "\\").Replace("/", "\\")}");

                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
                }
                catch (Exception ex)
                {
                    return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                    _logging.LogException(ex, new { id = item.Id });
                }
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UploadAvatar(UploadAvatarRequest request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
            var item = await _partnerRepository.FindAll(x => x.AccountId == accountId).FirstOrDefaultAsync();
            if (item == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\partner\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            IFormFile filesAvatar = request.File;

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/partner/avatar/{avatarUniqueFileName}";
            }

            try
            {
                _partnerRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                // Nếu tạo ra file rồi mã lưu db bị lỗi thì xóa file vừa tạo đi
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                // Không thêm được thì xóa file vừa tạo đi
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, item);
            }
            return processingResult;
        }
        public async Task<List<CustomerModel>> GetCustomersByPartnerId(int partnerId)
        {
            var data = await _customerPartnerRepository.FindAll(x => x.PartnerId == partnerId).ToListAsync();
            if (data.Count == 0)
                return new List<CustomerModel>();
            var customers = data.Select(x => x.Customer).ToList();

            if (customers.Count > 0)
            {
                var result = _mapper.Map<List<CustomerModel>>(customers);
                return result;
            }
            return new List<CustomerModel>();
        }
        public async Task<object> GetAllProductByAccount(int accountId)
        {
            var item = await _partnerRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (item == null) return new List<ProductModel>();
            return await _productRepository.FindAll()
           .Where(x => x.PartnerId == item.Id)
           .ProjectTo<ProductModel>(_configMapper)
           .ToListAsync();
        }
        public async Task<object> GetAllProductStopByAccount(int accountId)
        {
            var item = await _partnerRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (item == null) return new List<ProductModel>();
            return await _productRepository.FindAll()
           .Where(x => x.PartnerId == item.Id && x.Status == false)
           .ProjectTo<ProductModel>(_configMapper)
           .ToListAsync();
        }
        public async Task<object> GetAllProductIsSaleByAccount(int accountId)
        {
            var item = await _partnerRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (item == null) return new List<ProductModel>();
            return await _productRepository.FindAll()
           .Where(x => x.PartnerId == item.Id && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow)
           .ProjectTo<ProductModel>(_configMapper)
           .ToListAsync();
        }
    }
}
