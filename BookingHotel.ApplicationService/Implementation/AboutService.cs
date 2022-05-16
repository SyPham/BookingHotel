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

    public class AboutService : IAboutService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IBaseRepository<About> _aboutRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public AboutService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IBaseRepository<About> aboutRepository,
            IPartnerRepository partnerRepository,
            IHostingEnvironment currentEnvironment,
        IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _aboutRepository = aboutRepository;
            _partnerRepository = partnerRepository;
            _currentEnvironment = currentEnvironment;
            _unitOfWork = unitOfWork;
        }

        public async Task<AboutModel> FindById(int id)
        {
            var about = await _aboutRepository.FindAll().ProjectTo<AboutModel>(_configMapper).FirstOrDefaultAsync(x => x.Id == id);
          
            return about;
        }

        public async Task<AboutModel> FindByIdNoTracking(int id)
        {
            var item = await _aboutRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<AboutModel>(item);
        }

        public async Task<List<AboutModel>> GetAllAsync()
        {
            return await _aboutRepository.FindAll().ProjectTo<AboutModel>(_configMapper).ToListAsync();
        }


        public async Task<Pager> SearchPaginationAsync(SearchPagination paramater)
        {
            var query = _aboutRepository.FindAll(x => x.Title.Contains(paramater.strSearch)).ProjectTo<AboutModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(AboutModel model)
        {
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            IFormFile filesLogo = model.FilesLogo.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\about\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\about\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);


            var logoUniqueFileName = string.Empty;
            var logoFolderPath = "FileUploads\\images\\about\\logo";
            string uploadLogoFolder = Path.Combine(_currentEnvironment.WebRootPath, logoFolderPath);

            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            fileExtension.CreateFolder(uploadThumbFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/about/avatar/{avatarUniqueFileName}";
            }

            if (!filesThumb.IsNullOrEmpty())
            {
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                model.Thumb = $"/FileUploads/images/about/thumb/{thumbUniqueFileName}";
            }

            if (!filesLogo.IsNullOrEmpty())
            {
                logoUniqueFileName = await fileExtension.WriteAsync(filesLogo, $"{uploadLogoFolder}\\{logoUniqueFileName}");
                model.Logo = $"/FileUploads/images/about/logo/{logoUniqueFileName}";
            }

            var item = _mapper.Map<About>(model);
            try
            {
                _aboutRepository.Add(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
             
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                if (!thumbUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadThumbFolder}\\{thumbUniqueFileName}");

                if (!logoUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadLogoFolder}\\{logoUniqueFileName}");

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(AboutModel model)
        {
            // string token = Request.Headers["Authorization"];
            // var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (await FindByIdNoTracking(model.Id) == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            var item = _mapper.Map<About>(model);

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\about\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\about\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            var logoUniqueFileName = string.Empty;
            var logoFolderPath = "FileUploads\\images\\about\\logo";
            string uploadLogoFolder = Path.Combine(_currentEnvironment.WebRootPath, logoFolderPath);

            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            IFormFile filesLogo = model.FilesLogo.FirstOrDefault();

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/about/avatar/{avatarUniqueFileName}";
            }

            if (filesThumb != null)
            {
                if (!item.Thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Thumb.Replace("/", "\\").Replace("/", "\\")}");
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                item.Thumb = $"/FileUploads/images/about/thumb/{thumbUniqueFileName}";
            }

            if (filesLogo != null)
            {
                if (!item.Logo.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Logo.Replace("/", "\\").Replace("/", "\\")}");
                logoUniqueFileName = await fileExtension.WriteAsync(filesLogo, $"{uploadLogoFolder}\\{logoUniqueFileName}");
                item.Logo = $"/FileUploads/images/about/logo/{logoUniqueFileName}";
            }

            try
            {
                _aboutRepository.Update(item);
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

                if (!logoUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadLogoFolder}\\{logoUniqueFileName}");
                // Không thêm được thì xóa file vừa tạo đi
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = _aboutRepository.FindById(id);
            var avatar = item.Avatar;
            var thumb = item.Thumb;
            try
            {
                _aboutRepository.Remove(item);
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
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = id });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteRangeAsync(List<int> ids)
        {
            FileExtension fileExtension = new FileExtension();

            var query = await _aboutRepository.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var item in query)
            {
                try
                {
                    var avatar = item.Avatar;
                    var thumb = item.Thumb;
                    _aboutRepository.Remove(item);
                    await _unitOfWork.SaveAll();

                    if (!avatar.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");

                    if (!thumb.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{thumb.Replace("/", "\\").Replace("/", "\\")}");
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

        public Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            throw new NotImplementedException();
        }

        public async Task<AboutModel> GetFisrtAsync()
        {
            var about = await _aboutRepository.FindAll().ProjectTo<AboutModel>(_configMapper).FirstOrDefaultAsync();

            return about;
        }
    }
}
