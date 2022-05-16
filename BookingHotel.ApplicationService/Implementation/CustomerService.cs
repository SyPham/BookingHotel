using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly ILogging _logging;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductWishlistRepository _productWishlistRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public CustomerService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IHostingEnvironment currentEnvironment,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IHttpContextAccessor httpContextAccessor,
            IProductWishlistRepository productWishlistRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _logging = logging;
            _currentEnvironment = currentEnvironment;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _httpContextAccessor = httpContextAccessor;
            _productWishlistRepository = productWishlistRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(CustomerModel model)
        {
            var item = _mapper.Map<Customer>(model);
            try
            {
                _customerRepository.Add(item);
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
            var item = _customerRepository.FindById(id);
            try
            {
                _customerRepository.Remove(item);
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

        public async Task<ProcessingResult> Update2(CustomerModel model)
        {
            var item = await _customerRepository.FindAll().FirstOrDefaultAsync(x => x.Id == model.Id);
            try
            {
                item.Email = model.Email;
                item.FullName = model.FullName;
                item.Address = model.Address;
                if (model.Gender != null)
                    item.Gender = model.Gender;
                item.Birthday = model.Birthday;
                if (!string.IsNullOrEmpty(model.Avatar))
                    item.Avatar = model.Avatar;

                item.Phone = model.Phone;
                if (!string.IsNullOrEmpty(model.Tel))
                    item.Tel = model.Tel;
                item.Address = model.Address;
                if (!string.IsNullOrEmpty(model.CompanyName))
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
        public async Task<CustomerModel> FindById(int id)
        {
            var item = await _customerRepository.FindAll().Include(x=>x.Account).FirstOrDefaultAsync(x => x.Id == id);
            item.Account.Password = "";
            return _mapper.Map<CustomerModel>(item);
        }

        public async Task<List<CustomerModel>> GetAllAsync()
        {
            return await _customerRepository.FindAll().ProjectTo<CustomerModel>(_configMapper).ToListAsync();
        }

        public async Task<CustomerModel> GetByAccountAsync(int accountId)
        {
            var item = await _customerRepository.FindAll().Include(x => x.Account).FirstOrDefaultAsync(x => x.AccountId == accountId);
            item.Account.Password = "";
            return _mapper.Map<CustomerModel>(item);
        }

        public CustomerModel GetByAccount(int accountId)
        {
            var item = _customerRepository.FindAll().FirstOrDefault(x => x.AccountId == accountId);
            return _mapper.Map<CustomerModel>(item);
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _customerRepository.FindAll().ProjectTo<CustomerModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(CustomerModel model)
        {
            var item = _mapper.Map<Customer>(model);
            try
            {
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


        public async Task<ProcessingResult> UpdatePointAsync(int customerId, int point, int math) // 1 là công, 2 là trừ
        {
            try
            {
                var item = _customerRepository.FindById(customerId);
                if (math == 1)
                {
                    item.Point += point;
                }
                else if (math == 2)
                {
                    item.Point -= point;
                }

                _customerRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { customerId = customerId });
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
                var item = await _customerRepository.FindAll().FirstOrDefaultAsync(x => x.Email.Equals(value) && x.Id != id);
                if (item != null)
                    result = true;
            }
            else if (type == 2)
            {
                var item = await _customerRepository.FindAll().FirstOrDefaultAsync(x => x.Phone.Equals(value) && x.Id != id);
                if (item != null)
                    result = true;
            }
            return result;
        }

        public async Task<ProcessingResult> AddWithUploadAsync(CustomerModel model)
        {
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\customer\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);


            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/customer/avatar/{avatarUniqueFileName}";
            }


            var item = _mapper.Map<Customer>(model);
            try
            {
                _customerRepository.Add(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }
        public async Task<CustomerModel> FindByIdAsNoTracking(int id)
        {
            var item = await _customerRepository.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<CustomerModel>(item);
        }
        public async Task<ProcessingResult> UpdateWithUploadAsync(CustomerModel model)
        {
            // string token = Request.Headers["Authorization"];
            // var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (await FindByIdAsNoTracking(model.Id) == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            var item = _mapper.Map<Customer>(model);

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\customer\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/customer/avatar/{avatarUniqueFileName}";
            }

            try
            {
                _customerRepository.Update(item);
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
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteWithUploadAsync(int id)
        {
            var item = _customerRepository.FindById(id);
            var avatar = item.Avatar;
            try
            {
                _customerRepository.Remove(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                FileExtension fileExtension = new FileExtension();
                if (!avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { id = id });
            }
            return processingResult;
        }

        public async Task<ProcessingResult> DeleteRangeWithUploadAsync(List<int> ids)
        {
            FileExtension fileExtension = new FileExtension();

            var query = await _customerRepository.FindAll(x => ids.Contains(x.Id)).AsNoTracking().ToListAsync();
            foreach (var item in query)
            {
                try
                {
                    var avatar = item.Avatar;
                    _customerRepository.Remove(item);
                    await _unitOfWork.SaveAll();

                    if (!avatar.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{avatar.Replace("/", "\\").Replace("/", "\\")}");
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

        public async Task<List<OrderModel>> GetOrderByCustomerId(int customerId)
        {
            if (customerId == 0)
            {
                return new List<OrderModel>();
            }
            var data = await _orderRepository.FindAll(x => x.CustomerId == customerId)
                .Include(x => x.OrderDetails)
                .Include(x => x.OrderStatus)
                .Select(x => new OrderModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Code = x.Code,
                    Mobi = x.Mobi,
                    Email = x.Email,
                    OrderStatus = new OrderStatusModel { Title = x.OrderStatus != null ? x.OrderStatus.Title : "" },
                    Status = x.Status,
                    CreateTime = x.CreateTime,
                    PayType = new PayTypeModel { Title = x.PayType != null ? x.PayType.Title : "" },
                    OrderDetails = x.OrderDetails.Select(a => new OrderDetailModel
                    {
                        Product = new ProductModel
                        {
                            Code = x.PayType != null ? a.Product.Code : "",
                            Title = x.PayType != null ? a.Product.Title : ""
                        },
                        Price = a.Price,
                        Quantity = a.Quantity
                    }).ToList()
                }).ToListAsync();
            return data;
        }

        public async Task<List<ProductModel>> GetProductsWishlistByAccountId(int accountId)
        {

            if (accountId == 0)
            {
                return new List<ProductModel>();
            }
            var data = _productWishlistRepository.FindAll(x => x.AccountId == accountId)
              .Include(x => x.Product)
              .AsQueryable();
            var productsWishlist = await data.Select(x => x.Product).ProjectTo<ProductModel>(_configMapper).ToListAsync();
            return productsWishlist;
        }

        public async Task<List<ProductModel>> GetPurchasedProductsByCustomerId(int customerId)
        {
            var daThanhToanId = 2;
            var data = await _orderRepository.FindAll(x => x.CustomerId == customerId && x.OrderStatusId == daThanhToanId)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ToListAsync();
            if (data.Count == 0)
                return new List<ProductModel>();
            var details = data.SelectMany(x => x.OrderDetails).ToList();
            if (details.Count > 0)
            {
                var products = details.Select(x => x.Product).ToList();
                var result = _mapper.Map<List<ProductModel>>(products);
                return result;
            }
            return new List<ProductModel>();
        }

        public async Task<ProcessingResult> UploadAvatar(UploadAvatarRequest request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
            var item = await _customerRepository.FindAll(x => x.AccountId == accountId).FirstOrDefaultAsync();
            if (item == null)
            {
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\customer\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            IFormFile filesAvatar = request.File;

            if (filesAvatar != null)
            {
                if (!item.Avatar.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.Avatar.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.Avatar = $"/FileUploads/images/customer/avatar/{avatarUniqueFileName}";
            }

            try
            {
                _customerRepository.Update(item);
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

        public async Task<ProcessingResult> UpdateInfo(UpdateInfoRequest model)
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenById(token).ToInt();
            var item = await _customerRepository.FindAll(x => x.AccountId == accountId).FirstOrDefaultAsync();
            try
            {
                item.Email = model.Email;
                item.FullName = model.FullName;
                item.Birthday = model.Birthday;
                item.Gender = model.Gender;
                item.Address = model.Address;
                _customerRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, item);
            }
            return processingResult;
        }
    }
}
