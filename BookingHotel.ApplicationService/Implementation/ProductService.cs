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
using X.PagedList;

namespace BookingHotel.ApplicationService.Implementation
{

    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IProductRepository _productRepository;
        private readonly IProductTabRepository _productTabRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ITabRepository _tabRepository;
        private readonly ITabTypeRepository _tabTypeRepository;
        private readonly IProductWishlistRepository _productWishlistRepository;
        private readonly IProductWalletRepository _productWalletRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly IAssessRepository _assessRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public ProductService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IProductRepository productRepository,
            IProductTabRepository productTabRepository,
            IProductCategoryRepository productCategoryRepository,
            ITabRepository tabRepository,
            ITabTypeRepository tabTypeRepository,
            IProductWishlistRepository productWishlistRepository,
            IProductWalletRepository productWalletRepository,
            IPartnerRepository partnerRepository,
            IHttpContextAccessor httpContextAccessor,
            IAssessRepository assessRepository,
            IHostingEnvironment currentEnvironment,
        IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _productRepository = productRepository;
            _productTabRepository = productTabRepository;
            _productCategoryRepository = productCategoryRepository;
            _tabRepository = tabRepository;
            _tabTypeRepository = tabTypeRepository;
            _productWishlistRepository = productWishlistRepository;
            _productWalletRepository = productWalletRepository;
            _partnerRepository = partnerRepository;
            _httpContextAccessor = httpContextAccessor;
            _assessRepository = assessRepository;
            _currentEnvironment = currentEnvironment;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductModel> FindById(int id)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            
            int accountId =  JWTExtensions.GetDecodeTokenById(accessToken);
            ProductModel model = new ProductModel();
            var product = await _productRepository.FindAll()
                .Include(x => x.Partner)
                .Include(x => x.ProductWishlists)
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductTabs)
                .ThenInclude(x => x.Tab)
                .ThenInclude(x => x.TabType)
                .FirstOrDefaultAsync(x => x.Id == id);
            model = _mapper.Map<ProductModel>(product);
            model.ObjPartner = _mapper.Map<PartnerModel>(product.Partner);
            var wishlist = product.ProductWishlists.FirstOrDefault(x => x.AccountId == accountId);

            List<TabRequest> tabs = new List<TabRequest>();
            if (product.ProductTabs.Count > 0)
            {
                tabs = product.ProductTabs.Select(x => new TabRequest { TabId = x.TabId, Content = x.Tab.Content, TabTypeId = x.Tab.TabTypeId, Title = x.Tab.TabType.Title }).ToList();
            }
            model.TabRequest = tabs;
            //
            var assessList = _assessRepository.FindAll(x => x.KeyId == model.Id && x.KeyName == "Product");
            if (assessList != null)
                model.AssessRequest = assessList.OrderByDescending(x => x.NumberStar).Take(5).ProjectTo<AssessModel>(_configMapper).ToList();
            //
            if (wishlist != null) model.IsWishlist = true;

            return model;
        }

        // Chi tiết sản phẩm (kiểm tra sản phẩm này có nằm trong mục yêu thích của tài khoản hay không)
        public async Task<ProductModel> IsWishlistFindById(int id, int accountId)
        {
            ProductModel model = new ProductModel();
            var product = await _productRepository.FindAll()
                .Include(x => x.Partner)
                .Include(x => x.ProductWishlists)
                .Include(x => x.ProductTabs)
                .ThenInclude(x => x.Tab)
                .ThenInclude(x => x.TabType).FirstOrDefaultAsync(x => x.Id == id && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow);
            var wishlist = product.ProductWishlists.FirstOrDefault(x => x.AccountId == 3);
            if (wishlist != null) model.IsWishlist = true;

            model = _mapper.Map<ProductModel>(product);
            //
            model.ObjPartner = _mapper.Map<PartnerModel>(product.Partner);
            //
            List<TabRequest> tabs = new List<TabRequest>();
            if (product.ProductTabs.Count > 0)
            {
                tabs = product.ProductTabs.Select(x => new TabRequest { TabId = x.TabId, Content = x.Tab.Content, TabTypeId = x.Tab.TabTypeId, Title = x.Tab.TabType.Title }).ToList();
            }
            model.TabRequest = tabs;
            //
            var assessList = _assessRepository.FindAll(x => x.KeyId == model.Id && x.KeyName == "Product");
            if (assessList != null)
                model.AssessRequest = assessList.OrderByDescending(x => x.NumberStar).Take(5).ProjectTo<AssessModel>(_configMapper).ToList();
            //
            if (wishlist != null) model.IsWishlist = true;
            //
            return model;
        }

        public async Task<ProductModel> FindByIdNoTracking(int id)
        {
            var item = await _productRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ProductModel>(item);
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            return await _productRepository.FindAll(x=>x.Status == true)
                .Include(x=>x.ProductCategory)
                .Include(x=>x.Partner)
                .Include(x=>x.MethodUsed)
                .ProjectTo<ProductModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllByCategoryAsync(int catId)
        {
            if (catId == 0)
            {
            return await _productRepository.FindAll(x => x.ProductCategoryId > 0 && x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();

            }
            return await _productRepository.FindAll(x => x.ProductCategoryId == catId && x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();
        }
        public async Task<List<ProductCategoryHotModel>> GetSaleByCategoryAsync(int numberItem)
        {
            var products =  _productCategoryRepository.FindAll().Include(a => a.Products).ThenInclude(a => a.Partner).Take(numberItem).AsQueryable();
            return await products.Where(x => x.Status == true ).ProjectTo<ProductCategoryHotModel>(_configMapper).ToListAsync();
        }
        public async Task<List<ProductModel>> GetAllBySaleAsync(int saleId)
        {
            return await _productRepository.FindAll(x => x.SaleId == saleId && x.Status == true).Include(x=>x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllByUntilAsync(int untilId)
        {
            return await _productRepository.FindAll(x => x.UnitId == untilId && x.Status == true).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllByPartnerAsync(int partnerId)
        {
            if(partnerId == 0)
            {
                return await _productRepository.FindAll(x => x.PartnerId > 0 && x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();

            }
            return await _productRepository.FindAll(x => x.PartnerId == partnerId && x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();
        }

        public async Task<List<ProductModel>> GetByStatusAsync(bool status)
        {
            return await _productRepository.FindAll(x => x.Status == status && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _productRepository.FindAll(x => x.Status == paramater.status).ProjectTo<ProductModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<Pager> SearchPaginationAsync(SearchPagination paramater)
        {
            var query = _productRepository.FindAll(x => x.Title.Contains(paramater.strSearch) && x.Status == paramater.status && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).ProjectTo<ProductModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<Pager> FilterPaginationAsync(FilterPagination paramater)
        {
            if (paramater.condition == ProductEnum.MoiNhat)
            {
                var query = _productRepository.FindAll(x => x.Status == paramater.status 
                                                   && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow 
                                                   && (x.ProductCategoryId == paramater.catId || paramater.catId == 0) 
                                                   && (x.PartnerId == paramater.partnerId || paramater.partnerId == 0)).ProjectTo<ProductModel>(_configMapper);
                return await query.OrderByDescending(x => x.CreateTime).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
            else if (paramater.condition == ProductEnum.TangDan)
            {
                var query = _productRepository.FindAll(x => x.Status == paramater.status 
                                                    && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow 
                                                    && (x.ProductCategoryId == paramater.catId || paramater.catId == 0) 
                                                    && (x.PartnerId == paramater.partnerId || paramater.partnerId == 0)).ProjectTo<ProductModel>(_configMapper);
                return await query.OrderBy(x => x.OriginalPrice).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
            else if (paramater.condition == ProductEnum.GiamDan)
            {
                var query = _productRepository.FindAll(x => x.Status == paramater.status 
                                                    && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow 
                                                    && (x.ProductCategoryId == paramater.catId || paramater.catId == 0) 
                                                    && (x.PartnerId == paramater.partnerId || paramater.partnerId == 0)).ProjectTo<ProductModel>(_configMapper);
                return await query.OrderByDescending(x => x.OriginalPrice).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
            else
            {
                Random rand = new Random();
                var query = _productRepository.FindAll(x => x.Status == paramater.status 
                                                    && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow
                                                    && (x.ProductCategoryId == paramater.catId || paramater.catId == 0) 
                                                    && (x.PartnerId == paramater.partnerId || paramater.partnerId == 0)).ProjectTo<ProductModel>(_configMapper);

                int random = rand.Next(0, query.Count());
                return await query.OrderBy(x => Guid.NewGuid()).ToPaginationAsync(paramater.page, paramater.pageSize);
            }
        }

        public async Task<Pager> WishlistPaginationAsync(WishlistPagination paramater, int accountId)
        {
            // lấy danh sách id sản phẩm
            var idList = _productWishlistRepository.FindAll(x => x.AccountId == accountId).Select(x => x.ProductId);
            // lấy danh sách sản phẩm
            var query = _productRepository.FindAll(x => (idList.Any(xx => xx == x.Id)) && x.Status == paramater.status && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).ProjectTo<ProductModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }
        public async Task<object> GetWishlistAsync(int accountId)
        {
            // lấy danh sách id sản phẩm
            var idList = _productWishlistRepository.FindAll(x => x.AccountId == accountId).Select(x => x.ProductId);
            // lấy danh sách sản phẩm
            var query = _productRepository.FindAll(x => (idList.Any(xx => xx == x.Id)) && x.Status == true ).ProjectTo<ProductModel>(_configMapper);
            var data = await query.ToListAsync();
            foreach (var item in data)
            {
                item.IsWishlist= true;
            }
            return data;
        }
        public async Task<List<ProductModel>> GetAllHotAsync(int numberItem)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.ViewTime).Take(numberItem).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllNewAsync(int numberItem)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).Take(numberItem).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllRandomAsync(int numberItem)
        {
            Random rand = new Random();
            var query = _productRepository.FindAll(x => x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);

            int random = rand.Next(0, query.Count());
            return await query.OrderBy(x => Guid.NewGuid()).Take(numberItem).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllRelatedAsync(int id, int catId, int numberItem)
        {
            var query = _productRepository.FindAll(x => x.ProductCategoryId == catId && x.Id != id && x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllTopAsync(int numberItem)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllSaleAsync(int numberItem)
        {
            DateTime currentDate = DateTime.UtcNow.Date;
            var query = _productRepository.FindAll(x => x.Status == true && x.Sale > 0 && x.SaleStart <= DateTime.UtcNow && x.SaleDeadLine >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }
        public async Task<ProcessingResult> AddAsync(ProductModel model)
        {
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\product\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\product\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            fileExtension.CreateFolder(uploadThumbFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/product/avatar/{avatarUniqueFileName}";
            }

            if (!filesThumb.IsNullOrEmpty())
            {
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                model.Thumb = $"/FileUploads/images/product/thumb/{thumbUniqueFileName}";
            }
            var imageListFolderPath = "FileUploads\\images\\product\\imageList";
            string uploadImageListFolder = Path.Combine(_currentEnvironment.WebRootPath, imageListFolderPath);

            var imagesArray = new List<string> { };
            foreach (var imageItem in model.FilesImageList)
            {
                var imageListUniqueFileName = string.Empty;
                if (!imageItem.IsNullOrEmpty())
                {
                    imageListUniqueFileName = await fileExtension.WriteAsync(imageItem, $"{uploadImageListFolder}\\{imageListUniqueFileName}");
                    imagesArray.Add($"/FileUploads/images/product/imageList/{imageListUniqueFileName}");
                }
            }
            if (imagesArray.Count > 0)
            {
                model.ImageListProduct = string.Join(";", imagesArray.ToArray());
            }


            var item = _mapper.Map<Product>(model);
            try
            {
                _productRepository.Add(item);
                await _unitOfWork.SaveAll();

                var tabs = new List<Tab>();
                if (model.TabRequest != null)
                {
                    foreach (var itemTab in model.TabRequest)
                    {
                        var tab = new Tab
                        {
                            TabTypeId = itemTab.TabTypeId,
                            Content = itemTab.Content,
                            CreateTime = DateTime.UtcNow,
                            Status = true
                        };
                        tabs.Add(tab);
                    }
                    if (tabs.Count > 0)
                    {
                        _tabRepository.AddRange(tabs);
                        await _unitOfWork.SaveAll();
                        var productTabs = new List<ProductTab>();
                        foreach (var itemTab in tabs)
                        {
                            var productTab = new ProductTab
                            {
                                TabId = itemTab.Id,
                                ProductId = item.Id,
                                Value = itemTab.Content,
                                CreateTime = DateTime.UtcNow
                            };
                            productTabs.Add(productTab);
                        }
                        _productTabRepository.AddRange(productTabs);
                        await _unitOfWork.SaveAll();
                    }
                }


                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (imagesArray.Count > 0)
                {
                    foreach (var itemImages in imagesArray)
                    {
                        fileExtension.Remove($"{uploadImageListFolder}\\{itemImages}");
                    }
                }

                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                if (!thumbUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadThumbFolder}\\{thumbUniqueFileName}");
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(ProductModel model)
        {
            // string token = Request.Headers["Authorization"];
            // var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (await FindByIdNoTracking(model.Id) == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            var item = _mapper.Map<Product>(model);

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\product\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\product\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/product/avatar/{avatarUniqueFileName}";
            }

            if (filesThumb != null)
            {
                if (!item.Thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Thumb.Replace("/", "\\").Replace("/", "\\")}");
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                item.Thumb = $"/FileUploads/images/product/thumb/{thumbUniqueFileName}";
            }
            var imageListFolderPath = "FileUploads\\images\\product\\imageList";
            string uploadImageListFolder = Path.Combine(_currentEnvironment.WebRootPath, imageListFolderPath);

            var imagesArray = new List<string> { };
            foreach (var imageList in model.FilesImageList)
            {
                var imageListUniqueFileName = string.Empty;
                if (!model.ImageListProduct.IsNullOrEmpty() && model.ImageListProduct.Split(";").Count() > 0)
                {
                    // xoa het file cu
                    foreach (var itemImageList in model.ImageListProduct.Split(";"))
                    {
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{itemImageList.Replace("/", "\\").Replace("/", "\\")}");
                    }
                }
                if (!imageList.IsNullOrEmpty())
                {
                    imageListUniqueFileName = await fileExtension.WriteAsync(imageList, $"{uploadImageListFolder}\\{imageListUniqueFileName}");
                    imagesArray.Add($"/FileUploads/images/product/imageList/{imageListUniqueFileName}");
                }
            }
            if (imagesArray.Count > 0)
            {
                item.ImageListProduct = string.Join(";", imagesArray.ToArray());
            }
            try
            {
                _productRepository.Update(item);
                await _unitOfWork.SaveAll();

                var tabsUpdate = new List<Tab>();
                var tabsAdd = new List<Tab>();
                if (model.TabRequest != null)
                {
                    foreach (var itemTab in model.TabRequest)
                    {
                        var tab = _tabRepository.FindAll(x => x.Id == itemTab.TabId).FirstOrDefault();
                        if (tab != null)
                        {
                            tab.Content = itemTab.Content;
                            tabsUpdate.Add(tab);
                        }
                        else
                        {
                            var tabAdd = new Tab
                            {
                                TabTypeId = itemTab.TabTypeId,
                                Content = itemTab.Content,
                                CreateTime = DateTime.UtcNow,
                                Status = true
                            };
                            tabsAdd.Add(tabAdd);
                        }
                    }
                    if (tabsUpdate.Count > 0)
                    {
                        _tabRepository.UpdateRange(tabsUpdate);
                        await _unitOfWork.SaveAll();
                    }

                    if (tabsAdd.Count > 0)
                    {
                        _tabRepository.AddRange(tabsAdd);
                        await _unitOfWork.SaveAll();
                        var productTabs = new List<ProductTab>();
                        foreach (var itemTab in tabsAdd)
                        {
                            var productTab = new ProductTab
                            {
                                TabId = itemTab.Id,
                                ProductId = item.Id,
                                Value = itemTab.Content,
                                CreateTime = DateTime.UtcNow
                            };
                        }
                        _productTabRepository.AddRange(productTabs);
                        await _unitOfWork.SaveAll();
                    }
                }

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (imagesArray.Count > 0)
                {
                    foreach (var itemImages in imagesArray)
                    {
                        fileExtension.Remove($"{uploadImageListFolder}\\{itemImages}");
                    }
                }

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

        public async Task<ProcessingResult> AddAsync(ProductActionModel model)
        {
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\product\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\product\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            fileExtension.CreateFolder(uploadThumbFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/product/avatar/{avatarUniqueFileName}";
            }

            if (!filesThumb.IsNullOrEmpty())
            {
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                model.Thumb = $"/FileUploads/images/product/thumb/{thumbUniqueFileName}";
            }
            var imageListFolderPath = "FileUploads\\images\\product\\imageList";
            string uploadImageListFolder = Path.Combine(_currentEnvironment.WebRootPath, imageListFolderPath);

            var imagesArray = new List<string> { };
            foreach (var imageItem in model.FilesImageList)
            {
                var imageListUniqueFileName = string.Empty;
                if (!imageItem.IsNullOrEmpty())
                {
                    imageListUniqueFileName = await fileExtension.WriteAsync(imageItem, $"{uploadImageListFolder}\\{imageListUniqueFileName}");
                    imagesArray.Add($"/FileUploads/images/product/imageList/{imageListUniqueFileName}");
                }
            }
            if (imagesArray.Count > 0)
            {
                model.ImageListProduct = string.Join(";", imagesArray.ToArray());
            }


            var item = _mapper.Map<Product>(model);
            try
            {
                _productRepository.Add(item);
                await _unitOfWork.SaveAll();

                var tabs = new List<Tab>();
                if (model.TabRequest != null)
                {
                    foreach (var itemTab in model.TabRequest)
                    {
                        var tab = new Tab
                        {
                            TabTypeId = itemTab.TabTypeId,
                            Content = itemTab.Content,
                            CreateTime = DateTime.UtcNow,
                            Status = true
                        };
                        tabs.Add(tab);
                    }
                    if (tabs.Count > 0)
                    {
                        _tabRepository.AddRange(tabs);
                        await _unitOfWork.SaveAll();
                        var productTabs = new List<ProductTab>();
                        foreach (var itemTab in tabs)
                        {
                            var productTab = new ProductTab
                            {
                                TabId = itemTab.Id,
                                ProductId = item.Id,
                                Value = itemTab.Content,
                                CreateTime = DateTime.UtcNow
                            };
                            productTabs.Add(productTab);
                        }
                        _productTabRepository.AddRange(productTabs);
                        await _unitOfWork.SaveAll();
                    }
                }
               

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (imagesArray.Count > 0)
                {
                    foreach (var itemImages in imagesArray)
                    {
                        fileExtension.Remove($"{uploadImageListFolder}\\{itemImages}");
                    }
                }

                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                if (!thumbUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadThumbFolder}\\{thumbUniqueFileName}");
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAsync(ProductActionModel model)
        {
            // string token = Request.Headers["Authorization"];
            // var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (await FindByIdNoTracking(model.Id) == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            var item = _mapper.Map<Product>(model);

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\product\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\product\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);

            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/product/avatar/{avatarUniqueFileName}";
            }

            if (filesThumb != null)
            {
                if (!item.Thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Thumb.Replace("/", "\\").Replace("/", "\\")}");
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                item.Thumb = $"/FileUploads/images/product/thumb/{thumbUniqueFileName}";
            }
            var imageListFolderPath = "FileUploads\\images\\product\\imageList";
            string uploadImageListFolder = Path.Combine(_currentEnvironment.WebRootPath, imageListFolderPath);

            var imagesArray = new List<string> { };
            foreach (var imageList in model.FilesImageList)
            {
                var imageListUniqueFileName = string.Empty;
                if (!model.ImageListProduct.IsNullOrEmpty() && model.ImageListProduct.Split(";").Count() > 0)
                {
                    // xoa het file cu
                    foreach (var itemImageList in model.ImageListProduct.Split(";"))
                    {
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{itemImageList.Replace("/", "\\").Replace("/", "\\")}");
                    }
                }
                if (!imageList.IsNullOrEmpty())
                {
                    imageListUniqueFileName = await fileExtension.WriteAsync(imageList, $"{uploadImageListFolder}\\{imageListUniqueFileName}");
                    imagesArray.Add($"/FileUploads/images/product/imageList/{imageListUniqueFileName}");
                }
            }
            if (imagesArray.Count > 0)
            {
                item.ImageListProduct = string.Join(";", imagesArray.ToArray());
            }
            try
            {
                _productRepository.Update(item);
                await _unitOfWork.SaveAll();

                var tabsUpdate = new List<Tab>();
                var tabsAdd = new List<Tab>();
                if (model.TabRequest != null)
                {
                    foreach (var itemTab in model.TabRequest)
                    {
                        var tab = _tabRepository.FindAll(x => x.Id == itemTab.TabId).FirstOrDefault();
                        if (tab != null)
                        {
                            tab.Content = itemTab.Content;
                            tabsUpdate.Add(tab);
                        }
                        else
                        {
                            var tabAdd = new Tab
                            {
                                TabTypeId = itemTab.TabTypeId,
                                Content = itemTab.Content,
                                CreateTime = DateTime.UtcNow,
                                Status = true
                            };
                            tabsAdd.Add(tabAdd);
                        }
                    }
                    if (tabsUpdate.Count > 0)
                    {
                        _tabRepository.UpdateRange(tabsUpdate);
                        await _unitOfWork.SaveAll();
                    }

                    if (tabsAdd.Count > 0)
                    {
                        _tabRepository.AddRange(tabsAdd);
                        await _unitOfWork.SaveAll();
                        var productTabs = new List<ProductTab>();
                        foreach (var itemTab in tabsAdd)
                        {
                            var productTab = new ProductTab
                            {
                                TabId = itemTab.Id,
                                ProductId = item.Id,
                                Value = itemTab.Content,
                                CreateTime = DateTime.UtcNow
                            };
                        }
                        _productTabRepository.AddRange(productTabs);
                        await _unitOfWork.SaveAll();
                    }
                }
              
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (imagesArray.Count > 0)
                {
                    foreach (var itemImages in imagesArray)
                    {
                        fileExtension.Remove($"{uploadImageListFolder}\\{itemImages}");
                    }
                }

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
            var item = _productRepository.FindById(id);
            try
            {
                item.Status = item.Status == true ? false : true;
                _productRepository.Update(item);
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
            var item = _productRepository.FindById(id);
            try
            {
                if (item.ViewTime.HasValue && item.ViewTime.Value > 0) item.ViewTime = item.ViewTime + 1; else  item.ViewTime = 1;
                _productRepository.Update(item);
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
        public async Task<ProcessingResult> UpdateValueAssessAsync(ValueAssessRequest request)
        {
            var item = await _productRepository.FindAll(x=> x.Id == request.Id).FirstOrDefaultAsync();
            try
            {
                item.ValueAssess = request.ValueAssess;
                _productRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = request.Id });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = _productRepository.FindById(id);
            var avatar = item.Avatar;
            var thumb = item.Thumb;
            try
            {
                var tabs = await _productTabRepository.FindAll(x => x.ProductId == id).ToListAsync();
                _productTabRepository.RemoveMultiple(tabs);
                await _unitOfWork.SaveAll();
                _productRepository.Remove(item);
                await _unitOfWork.SaveAll();
              
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
                FileExtension fileExtension = new FileExtension();
                if (!avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");

                if (!thumb.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{thumb.Replace("/", "\\").Replace("/", "\\")}");
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
            FileExtension fileExtension = new FileExtension();

            var query = await _productRepository.FindAll(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var item in query)
            {
                try
                {
                    var avatar = item.Avatar;
                    var thumb = item.Thumb;
                    var tabs = await _productTabRepository.FindAll(x => x.ProductId == item.Id).ToListAsync();
                    _productTabRepository.RemoveMultiple(tabs);
                    await _unitOfWork.SaveAll();
                    _productRepository.Remove(item);
                    await _unitOfWork.SaveAll();
                  
                    if (!avatar.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");

                    if (!thumb.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{thumb.Replace("/", "\\").Replace("/", "\\")}");
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
                }
                catch (Exception ex)
                {
                    _logging.LogException(ex, new { id = item.Id });
                    return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                }
            }
            return processingResult;
        }

        public async Task<object> LoadData(FilterRequest request)
        {
            try
            {
                
            int page = request.Page == 0 ? 1 : request.Page;
            int pageSize = request.Page == 0 ? 9 : request.PageSize;
            var list = new List<ProductModel>();
            var rate = request.Rate.Select(i => (decimal)i).ToList();
            if (request.catIds.Count > 0 && request.PartnerIds.Count > 0 && request.Rate.Count > 0)
            {
                 list = await  _productRepository.FindAll(x=>x.Status.Value 
                 && request.catIds.Contains(x.ProductCategoryId.Value)
                 && request.PartnerIds.Contains(x.PartnerId.Value) )
                .Include(x=>x.ProductCategory)
                .Include(x=>x.Partner)
                .Include(x=>x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                .ToListAsync();
                list = list.Where(x=> rate.Any(w => w <= x.ValueAssess)).ToList();

            } else if (request.catIds.Count > 0 && request.PartnerIds.Count == 0&& request.Rate.Count > 0)
            {
                  list = await  _productRepository.FindAll(x=>x.Status.Value 
                 && request.catIds.Contains(x.ProductCategoryId.Value)
                )
                .Include(x=>x.ProductCategory)
                .Include(x=>x.Partner)
                .Include(x=>x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                .ToListAsync();
                list = list.Where(x=> rate.Any(w => w <= x.ValueAssess)).ToList();

            }
              else if (request.catIds.Count == 0 && request.PartnerIds.Count > 0 && request.Rate.Count > 0)
            {
                  list = await  _productRepository.FindAll(x=>x.Status.Value 
                 && request.PartnerIds.Contains(x.PartnerId.Value) 
                )
                .Include(x=>x.ProductCategory)
                .Include(x=>x.Partner)
                .Include(x=>x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                .ToListAsync();
                list = list.Where(x=> rate.Any(w => w <= x.ValueAssess)).ToList();

            }
                else if (request.catIds.Count > 0 && request.PartnerIds.Count > 0 && request.Rate.Count == 0)
                {
                    list = await _productRepository.FindAll(x => x.Status.Value
                  && request.catIds.Contains(x.ProductCategoryId.Value)
                  && request.PartnerIds.Contains(x.PartnerId.Value)
                  )
                  .Include(x => x.ProductCategory)
                  .Include(x => x.Partner)
                  .Include(x => x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                  .ToListAsync();
                }
                else if (request.catIds.Count == 0 && request.PartnerIds.Count > 0 && request.Rate.Count == 0)
                {
                    list = await _productRepository.FindAll(x => x.Status.Value
                  && request.PartnerIds.Contains(x.PartnerId.Value)
                  )
                  .Include(x => x.ProductCategory)
                  .Include(x => x.Partner)
                  .Include(x => x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                  .ToListAsync();

                }
                else if (request.catIds.Count > 0 && request.PartnerIds.Count == 0 && request.Rate.Count == 0)
                {
                    list = await _productRepository.FindAll(x => x.Status.Value
                  && request.catIds.Contains(x.ProductCategoryId.Value)
                  )
                  .Include(x => x.ProductCategory)
                  .Include(x => x.Partner)
                  .Include(x => x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                  .ToListAsync();
                }
                else 
            {
                list = await  _productRepository.FindAll(x=>x.Status.Value)
                .Include(x=>x.ProductCategory)
                .Include(x=>x.Partner)
                .Include(x=>x.MethodUsed).ProjectTo<ProductModel>(_configMapper)
                .ToListAsync();
                if (rate.Count > 0)
                list = list.Where(x=> rate.Any(w => w <= x.ValueAssess)).ToList();

            }
            
            if (request.Increase)
            {
                list = list.OrderBy(x => x.OriginalPrice).ToList();
            }
            if (request.Decrease)
            {
                list = list.OrderByDescending(x => x.OriginalPrice).ToList();
            }
             if (request.Percent)
            {
                list = list.OrderByDescending(x => x.Sale).ToList();
            }
             int totalRow = list.Count();
            var res =  list.Skip((page - 1) * pageSize)
            .Take(pageSize).ToList();
            return new
            {
                data = res,
                total = totalRow,
                status = true,
                page,
                pageSize
            };
            }
            catch(Exception ex)
            {
                 return new
            {
                
                status = false,
               
            };
            } 
        }

        public async Task<object> Search(int catId, string keyWord)
        {

            if (catId == 0)
            {
                var query = await _productRepository.FindAll(x => (x.Title.Contains(keyWord) ||  x.Description.Contains(keyWord)) && x.Status == true ).ProjectTo<ProductModel>(_configMapper).ToListAsync();
                return query;
            } else
            {
                var query = await _productRepository.FindAll(x => x.ProductCategoryId == catId && (x.Title.Contains(keyWord) ||  x.Description.Contains(keyWord)) && x.Status == true ).ProjectTo<ProductModel>(_configMapper).ToListAsync();
                return query;
            }
         
        }

        public async Task<object> GetAllProductByAccount(int accountId)
        {
                        return await _productWalletRepository.FindAll()
                       .Where(x => x.AccountId == accountId)
                       .Include(x=>x.Product)
                       .Select(x => x.Product)
                       .ProjectTo<ProductModel>(_configMapper)
                       .ToListAsync();
        }

        public async Task<object> GetAllProductIsUseByAccount(int accountId)
        {
                 return await _productWalletRepository.FindAll()
                       .Where(x => x.AccountId == accountId && x.IsUse == true)
                       .Include(x=>x.Product)
                       .Select(x => x.Product)
                       .ProjectTo<ProductModel>(_configMapper)
                       .ToListAsync();
        }

        public async Task<object> GetAllProductNotUseByAccount(int accountId)
        {
                 return await _productWalletRepository.FindAll()
                       .Where(x => x.AccountId == accountId && x.IsUse == false)
                       .Include(x=>x.Product)
                       .Select(x => x.Product)
                       .ProjectTo<ProductModel>(_configMapper)
                       .ToListAsync();
        }

        public async Task<List<ProductModel>> GetAllBestSellerAsync(int numberItem)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.SoldView > 0 && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.Position).Take(numberItem).ToListAsync();
        }

        public async Task<List<ProductModel>> GetFeaturedProducts(int numberItem)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.CreateTime).Take(numberItem).ToListAsync();
        }

        public async Task<object> FilterNameAZ(int catId)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.ProductCategoryId.Value == catId && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderBy(x => x.Title).ToListAsync();
        }

        public async Task<object> FilterNameZA(int catId)
        {
            var query = _productRepository.FindAll(x => x.Status == true && x.ProductCategoryId.Value == catId && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.Title).ToListAsync();
        }

        public async Task<object> FilterPriceAZ(int catId)
        {

            var query = _productRepository.FindAll(x => x.Status == true && x.ProductCategoryId.Value == catId && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return (await query.ToListAsync()).OrderBy(x => x.Price);
        }

        public async Task<object> FilterPriceZA(int catId)
        {

            var query = _productRepository.FindAll(x => x.Status == true && x.ProductCategoryId.Value == catId && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return (await query.ToListAsync()).OrderByDescending(x => x.Price);
        }

        public async Task<object> FilterRatingAZ(int catId)
        {

            var query = _productRepository.FindAll(x => x.Status == true && x.ProductCategoryId.Value == catId && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderBy(x => x.ValueAssess).ToListAsync();
        }

        public async Task<object> FilterRatingZA(int catId)
        {

            var query = _productRepository.FindAll(x => x.Status == true && x.ProductCategoryId.Value == catId && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Include(x => x.Partner).ProjectTo<ProductModel>(_configMapper);
            return await query.OrderByDescending(x => x.ValueAssess).ToListAsync();
        }
    }
}
