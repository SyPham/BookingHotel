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
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly IArticleRepository _articleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ArticleService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IHostingEnvironment currentEnvironment,
            IArticleRepository articleRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ArticleModel> FindById(int id)
        {
            var item = await _articleRepository.FindAll().Include(x=>x.ArticleCategory).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ArticleModel>(item);
        }

        public async Task<ArticleModel> FindByIdNoTracking(int id)
        {
            var item = await _articleRepository.FindAll().Include(x=>x.ArticleCategory).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ArticleModel>(item);
        }

        public async Task<List<ArticleModel>> GetAllAsync()
        {
            return await _articleRepository.FindAll().Include(x=>x.ArticleCategory).ProjectTo<ArticleModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ArticleModel>> GetAllByCategoryAsync(int catId)
        {
            return await _articleRepository.FindAll(x => x.ArticleCategoryId == catId && x.Status == true).ProjectTo<ArticleModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ArticleModel>> GetByStatusAsync(bool status)
        {
            return await _articleRepository.FindAll(x => x.Status == status).ProjectTo<ArticleModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _articleRepository.FindAll().ProjectTo<ArticleModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<List<ArticleModel>> GetAllHotAsync(int numberItem)
        {
            var query = _articleRepository.FindAll(x => x.Status == true).ProjectTo<ArticleModel>(_configMapper);
            return await query.OrderByDescending(x => x.ViewTime).Take(numberItem).ToListAsync();
        }

        public async Task<List<ArticleModel>> GetAllNewAsync(int numberItem)
        {
            var query = _articleRepository.FindAll(x => x.Status == true).ProjectTo<ArticleModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).Take(numberItem).ToListAsync();
        }

        public async Task<List<ArticleModel>> GetAllRandomAsync(int numberItem)
        {
            Random rand = new Random();
            var query = _articleRepository.FindAll(x => x.Status == true).ProjectTo<ArticleModel>(_configMapper);

            int random = rand.Next(0, query.Count());
            return await query.OrderBy(x => Guid.NewGuid()).Take(numberItem).ToListAsync();
        }

        public async Task<List<ArticleModel>> GetAllRelatedAsync(int id, int catId, int numberItem)
        {
            var query = _articleRepository.FindAll(x => x.ArticleCategoryId == catId && x.Id != id && x.Status == true).ProjectTo<ArticleModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }

        public async Task<List<ArticleModel>> GetAllTopAsync(int numberItem)
        {
            var query = _articleRepository.FindAll(x => x.Status == true).ProjectTo<ArticleModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }

        public async Task<ProcessingResult> AddAsync(ArticleModel model)
        {
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\article\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\article\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);


            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            fileExtension.CreateFolder(uploadThumbFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/article/avatar/{avatarUniqueFileName}";
            }

            if (!filesThumb.IsNullOrEmpty())
            {
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                model.Thumb = $"/FileUploads/images/article/thumb/{thumbUniqueFileName}";
            }
            var item = _mapper.Map<Article>(model);
            try
            {
                _articleRepository.Add(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                if (!thumbUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadThumbFolder}\\{thumbUniqueFileName}");
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(ArticleModel model)
        {
            // string token = Request.Headers["Authorization"];
            // var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (await FindById(model.Id) == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            var item = _mapper.Map<Article>(model);

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\article\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\article\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/article/avatar/{avatarUniqueFileName}";
            }

            if (filesThumb != null)
            {
                if (!item.Thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Thumb.Replace("/", "\\").Replace("/", "\\")}");
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                item.Thumb = $"/FileUploads/images/article/thumb/{thumbUniqueFileName}";
            }

            try
            {
                _articleRepository.Update(item);
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
                // Không thêm được thì xóa file vừa tạo đi
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateStatusAsync(int id)
        {
            var item = _articleRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _articleRepository.Update(item);
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

        public async Task<ProcessingResult> UpdateViewAsync(int id)
        {
            var item = _articleRepository.FindById(id);
            try
            {
                item.ViewTime += item.ViewTime;
                _articleRepository.Update(item);
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
            var item = _articleRepository.FindById(id);
            var avatar = item.Avatar;
            var thumb = item.Thumb;
            try
            {
                _articleRepository.Remove(item);
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

            var query = await _articleRepository.FindAll(x => ids.Contains( x.Id)).ToListAsync();
            foreach (var item in query)
            {
                try
                {
                    var avatar = item.Avatar;
                    var thumb = item.Thumb;
                    _articleRepository.Remove(item);
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
    }
}
