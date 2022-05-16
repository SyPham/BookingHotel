using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Catalog;
using System.Linq;

namespace BookingHotel.ApplicationService.Implementation
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IArticleCategoryRepository _articleCatRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ArticleCategoryService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IArticleCategoryRepository articleCatRepository,
            IArticleRepository articleRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _articleCatRepository = articleCatRepository;
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ArticleCategoryModel> FindById(int id)
        {
            var item = await _articleCatRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ArticleCategoryModel>(item);
        }

        public async Task<List<ArticleCategoryModel>> GetAllAsync()
        {
            return await _articleCatRepository.FindAll().ProjectTo<ArticleCategoryModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ArticleCategoryModel>> GetAllGroupAsync(int parentId)
        {
            return await _articleCatRepository.FindAll(x => x.ParentId == parentId).ProjectTo<ArticleCategoryModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ArticleCategoryModel>> GetByStatusAsync(bool status)
        {
            return await _articleCatRepository.FindAll(x => x.Status == status).ProjectTo<ArticleCategoryModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _articleCatRepository.FindAll().ProjectTo<ArticleCategoryModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> AddAsync(ArticleCategoryModel model)
        {
            var item = _mapper.Map<ArticleCategory>(model);
            try
            {
                _articleCatRepository.Add(item);
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

        public async Task<ProcessingResult> UpdateAsync(ArticleCategoryModel model)
        {
            var item = _mapper.Map<ArticleCategory>(model);
            try
            {
                _articleCatRepository.Update(item);
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
            var item = _articleCatRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _articleCatRepository.Update(item);
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
            var item = _articleCatRepository.FindById(id);
            try
            {
                _articleCatRepository.Remove(item);
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
            var item = await _articleCatRepository.FindAll(x=> ids.Contains(x.Id)).Include(x=>x.Articles).ToListAsync();
            try
            {
                var articles = item.SelectMany(x => x.Articles).ToList();
                _articleRepository.RemoveMultiple(articles);
                _articleCatRepository.RemoveMultiple(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { ids = ids });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateStatus(int id)
        {

            var item = _articleCatRepository.FindById(id);
            try
            {
                item.Status = !item.Status;
                _articleCatRepository.Update(item);
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
