using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation.Mobile
{
    public interface IMobileBaseService
    {
        Task<List<NotificationModel>> GetAllNotificationAsync();
        Task<NotificationModel> GetByIDNotificationAsync(int Id);
        Task<ProcessingResult> Register(RegisterPartnerRequest request);
        Task<ProcessingResult> ResetPassword(ResetPasswordRequest model);
        Task<ProcessingResult> UpdateAccountStatus(int accountId);
        Task<ProcessingResult> ChangePassword(ChangePasswordRequest request);
        Task<AuthenticateResponse> Authenticate(AuthenticatePartnerRequest model, string ipAddress, int accountTypeId);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        Task<bool> RevokeToken(string token, string ipAddress);
        Task<ProcessingResult> ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<ProcessingResult> UpdatePartner(PartnerModel model);
        Task<ProcessingResult> AddFeedback(FeedbackModel model);
        Task<ProcessingResult> NotificationSetting(NotificationAccountModel model);
        Task<ProcessingResult> VerifyEmail(string token);
        Task<ProcessingResult> SendTokenVerifyEmail(int accountId, string email, string origin);
        PartnerModel GetByAccount(int accountId);

        Task<List<ProductModel>> GetAllProductByPartnerId(int partnerId);
        Task<ProductModel> GetByProductId(int productId);
        Task<List<Customer>> GetAllCustomerByPartnerId(int partnerId);
        Task<List<FeedbackModel>> GetAllFeedback();
        Task<object> GetViewTimeProductTop10ByPartnerId(int partnerId);
        Task<object> GetSoldViewProductTop10ByPartnerId(int partnerId);

    }
    public class MobileBaseService : IMobileBaseService
    {
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IBaseRepository<Feedback> _feedbackRepository;
        private readonly IBaseRepository<CustomerPartner> _customerPartnerRepository;
        private readonly IBaseRepository<Partner> _partnerRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<NotificationAccount> _notificationAccountRepository;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly ILogging _logging;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly MapperConfiguration _configMapper;
        private readonly IBaseRepository<Account> _accountRepository;
        private ProcessingResult processingResult;
        private readonly AppSettings _appSettings;

        public MobileBaseService(
            IBaseRepository<Notification> notificationRepository,
            IBaseRepository<Feedback> feedbackRepository,
            IBaseRepository<CustomerPartner> customerPartnerRepository,
            IBaseRepository<Partner> partnerRepository,
            IBaseRepository<Product> productRepository,
            IUnitOfWork unitOfWork,
            IBaseRepository<NotificationAccount> notificationAccountRepository,
            IHostingEnvironment currentEnvironment,
            ILogging logging,
             IMapper mapper,
       IEmailService emailService,

        MapperConfiguration configMapper,
            IOptions<AppSettings> appSettings,
            IBaseRepository<Account> accountRepository
            )
        {
            _appSettings = appSettings.Value;
            _notificationRepository = notificationRepository;
            _feedbackRepository = feedbackRepository;
            _customerPartnerRepository = customerPartnerRepository;
            _partnerRepository = partnerRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _notificationAccountRepository = notificationAccountRepository;
            _currentEnvironment = currentEnvironment;
            _logging = logging;
            _mapper = mapper;
            _emailService = emailService;
            _configMapper = configMapper;
            _accountRepository = accountRepository;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticatePartnerRequest model, string ipAddress, int accountTypeId)
        {
            // return null if user not found
            var user = _accountRepository.FindAll().Include(x => x.RefreshTokens).SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password && x.AccountTypeId == accountTypeId);
            if (user == null) return null;

            var partner = _partnerRepository.FindAll().SingleOrDefault(x => x.AccountId == user.Id);
            if (partner == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user, partner.Id);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            user.RefreshTokens.Add(refreshToken);
            _accountRepository.Update(user);
            await _unitOfWork.SaveAll();

            return new AuthenticateResponse(user, partner.Title, jwtToken, refreshToken.Token);
        }

        public async Task<ProcessingResult> ChangePassword(ChangePasswordRequest request)
        {
            var item = await _accountRepository.FindAll(x => x.Password == request.OldPassword && x.Id == request.Id).FirstOrDefaultAsync();
            if (item == null)
                return new ProcessingResult { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };

            try
            {
                item.Password = request.NewPassword;
                _accountRepository.Update(item);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, request);
            }
            return processingResult;
        }

        public async Task<List<NotificationModel>> GetAllNotificationAsync()
        {
            return await _notificationRepository.FindAll().ProjectTo<NotificationModel>(_configMapper).ToListAsync();

        }

        public async Task<NotificationModel> GetByIDNotificationAsync(int Id)
        {
            var item = await _notificationRepository.FindAll().FirstOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<NotificationModel>(item);
        }

        public async Task<ProcessingResult> Register(RegisterPartnerRequest request)
        {
            var item = new Account
            {
                UserName = request.Username,
                Password = request.Password,
                Status = true,
                CreateTime = DateTime.Now,
                AccountTypeId = 3
            };

            try
            {
                _accountRepository.Add(item);

                await _unitOfWork.SaveAll();
                var item2 = new Partner
                {
                    Title = request.Title,
                    AccountId = item.Id,
                    PartnerTypeId = request.PartnerTypeId,
                    Email = request.Email
                };
                _partnerRepository.Add(item2);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            return processingResult;
        }

        public async Task<ProcessingResult> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _accountRepository.FindAll().SingleOrDefaultAsync(x =>
             x.ResetToken == model.Token &&
             x.ResetTokenExpiresTime > DateTime.UtcNow);

            if (account == null)
                return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false };
            try
            {
                // update password and remove reset token
                account.Password = model.Password;
                account.PasswordResetTime = DateTime.UtcNow;
                account.ResetToken = null;
                account.ResetTokenExpiresTime = null;

                _accountRepository.Update(account);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "Đổi mật khẩu thành công", Success = true, Data = account };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public PartnerModel GetByAccount(int accountId)
        {
            var item = _partnerRepository.FindAll().FirstOrDefault(x => x.AccountId == accountId);
            return _mapper.Map<PartnerModel>(item);
        }
        private string generateJwtToken(Account user, int partnerId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("PartnerId", partnerId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    CreateTime = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        private void removeOldRefreshTokens(Account account)
        {
            var ressult = account.RefreshTokens.Where(x =>
                  !x.IsActive &&
                  x.CreateTime.GetValueOrDefault().AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private string randomTokenString(int maxlength = 5)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, maxlength).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
        public async Task<ProcessingResult> VerifyEmail(string token)
        {
            var account = _accountRepository.FindAll().SingleOrDefault(x => x.VerificationToken == token &&
                x.VerificationTokenExpiresTime > DateTime.UtcNow);

            if (account == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Xác thực thất bại", Success = false };
            try
            {
                account.VerifiedTime = DateTime.UtcNow;
                account.VerificationToken = null;

                _accountRepository.Update(account);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "Xác thực thành công", Success = true, Data = account };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new
                {
                    token = token
                });
            }
            return processingResult;
        }
        public async Task<ProcessingResult> SendTokenVerifyEmail(int accountId, string email, string origin)
        {
            var account = _accountRepository.FindAll().SingleOrDefault(x => x.Id == accountId);
            if (account == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Xác thực thất bại", Success = false };
            try
            {
                account.VerificationToken = randomTokenString(6);
                account.VerificationTokenExpiresTime = DateTime.UtcNow.AddMinutes(5);

                _accountRepository.Update(account);
                await _unitOfWork.SaveAll();

                // send email
                sendVerificationEmail(account, email, origin);
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "Vui lòng kiểm tra email", Success = true, Data = account };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, new { accountId = accountId, email = email });
            }
            return processingResult;
        }
        private void sendVerificationEmail(Account account, string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/partnerauth/verifyemail?token={account.VerificationToken}";
                message = $@"<p>Vui lòng nhấp vào liên kết dưới đây để xác thực địa chỉ email của bạn:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Đây là mã kích hoạt tài khoản của bạn:</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            _emailService.Send(
                to: email,
                subject: "[Vietcoupon] Mã xác thực tài khoản",
                html: $@"<h4>Chào mừng bạn đến với Vietcoupon!</h4>
                         {message}
                        <p>Đây là một tin nhắn tự động, vui lòng không trả lời!</p>"
            );
        }

        private void sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>Nếu bạn không biết mật khẩu của mình, vui lòng truy cập <a href=""{origin}/partnerauth/forgotpassword"">Quên mật khẩu</a>.</p>";
            else
                message = "<p>Nếu bạn quên mật khẩu của mình, bạn có chọn <strong>Quên mật khẩu</strong>.</p>";

            _emailService.Send(
                to: email,
                subject: "[Vietcoupon] Đăng ký tài khoản thành công",
                html: $@"<h4>Email đã đăng ký</h4>
                         <p>Email của bạn <strong> {email} </strong> đã được đăng ký.</p>
                         {message}"
            );
        }

        private void sendPasswordResetEmail(Account account, string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/partnerauth/resetpassword?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Đây là mã xác thực dùng để lấy lại mật khẩu:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: email,
                subject: "[Vietcoupon] Mã xác thực lấy lại mật khẩu",
                html: $@"<h4>Lấy lại mật khẩu</h4>
                         {message}"
            );
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var account = await _accountRepository.FindAll().Include(x => x.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (account == null) return null;

            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.RevokeTime = DateTime.Now;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);
            _accountRepository.Update(account);
            _unitOfWork.Save();
            var partner = _partnerRepository.FindAll().SingleOrDefault(x => x.AccountId == account.Id);
            // generate new jwt
            var jwtToken = generateJwtToken(account, partner.Id);
            var data = new AuthenticateResponse(account, "", jwtToken, newRefreshToken.Token);
            // data.Permissions = await _accountService.GetActionFunctionByAccountId(account.Id);
            //data.Menus = await _accountService.GetMenusByAccountId(account.Id);
            return data;
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {

            var user = await _accountRepository.FindAll().Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token);
            if (refreshToken == null) return false;
            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.RevokeTime = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _accountRepository.Update(user);
            await _unitOfWork.SaveAll();

            return true;
        }

        public async Task<ProcessingResult> UpdatePartner(PartnerModel model)
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

        public async Task<ProcessingResult> AddFeedback(FeedbackModel model)
        {
            model.ModifyTime = null;
            IFormFile filesAvatar = model.FilesAvatar.FirstOrDefault();
            IFormFile filesThumb = model.FilesThumb.FirstOrDefault();
            // Ghi file
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\feedback\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            var thumbUniqueFileName = string.Empty;
            var thumbFolderPath = "FileUploads\\images\\feedback\\thumb";
            string uploadThumbFolder = Path.Combine(_currentEnvironment.WebRootPath, thumbFolderPath);


            FileExtension fileExtension = new FileExtension();
            fileExtension.CreateFolder(uploadAvatarFolder);
            fileExtension.CreateFolder(uploadThumbFolder);
            if (!filesAvatar.IsNullOrEmpty())
            {
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                model.Avatar = $"/FileUploads/images/feedback/avatar/{avatarUniqueFileName}";
            }

            if (!filesThumb.IsNullOrEmpty())
            {
                thumbUniqueFileName = await fileExtension.WriteAsync(filesThumb, $"{uploadThumbFolder}\\{thumbUniqueFileName}");
                model.Thumb = $"/FileUploads/images/feedback/thumb/{thumbUniqueFileName}";
            }
            var item = _mapper.Map<Feedback>(model);
            try
            {
                _feedbackRepository.Add(item);
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

        public async Task<ProcessingResult> NotificationSetting(NotificationAccountModel model)
        {
            try
            {
                var check = _notificationAccountRepository.FindAll(x => x.AccountId == model.AccountId && x.NotificationId == model.NotificationId).FirstOrDefault();
                // Neu co trong db thi la tat thong bao, nguoc lai bat thong bao
                if (check != null)
                {
                    _notificationAccountRepository.Remove(check);
                    await _unitOfWork.SaveAll();
                }
                else
                {
                    var item = new NotificationAccount
                    {
                        AccountId = model.AccountId,
                        NotificationId = model.NotificationId,
                        CreateTime = DateTime.Now,
                    };

                    _notificationAccountRepository.Add(item);
                    await _unitOfWork.SaveAll();
                }

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = model };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            return processingResult;
        }

        public async Task<ProcessingResult> ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var partner = _partnerRepository.FindAll().SingleOrDefault(x => x.Email == model.Email);
            if (partner == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false };

            var account = _accountRepository.FindAll().SingleOrDefault(x => x.Id == partner.AccountId);
            if (account == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false };
            try
            {
                // create reset token that expires after 1 day
                account.ResetToken = randomTokenString(6);
                account.ResetTokenExpiresTime = DateTime.UtcNow.AddDays(1);

                _accountRepository.Update(account);
                await _unitOfWork.SaveAll();

                // send email
                sendPasswordResetEmail(account, model.Email, origin);
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "Vui lòng kiểm tra email", Success = true, Data = account };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> UpdateAccountStatus(int accountId)
        {
                var item = await _accountRepository.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
            if (item == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            try
            {
                item.Status = !item.Status;

                _accountRepository.Add(item);
                await _unitOfWork.SaveAll();

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            return processingResult;
        }

        public async Task<List<ProductModel>> GetAllProductByPartnerId(int partnerId)
        {
            var data = await _productRepository.FindAll(x => x.PartnerId == partnerId)
              .ProjectTo<ProductModel>(_configMapper).ToListAsync();
            return data;
        }

        public async Task<ProductModel> GetByProductId(int productId)
        {
            var data = await _productRepository.FindAll(x => x.Id == productId)
             .ProjectTo<ProductModel>(_configMapper).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<Customer>> GetAllCustomerByPartnerId(int partnerId)
        {
            var data = await _customerPartnerRepository.FindAll(x => x.PartnerId == partnerId)
                .Include(x => x.Customer)
                .Select(x => x.Customer).ToListAsync();
            return data;
        }

        public async Task<List<FeedbackModel>> GetAllFeedback()
        {
            var data = await _feedbackRepository.FindAll()
              .ProjectTo<FeedbackModel>(_configMapper).ToListAsync();
            return data;
        }

        public async Task<object> GetViewTimeProductTop10ByPartnerId(int partnerId)
        {
            var data = await _productRepository.FindAll(x => x.ViewTime > 0 && x.PartnerId == partnerId)
             .ProjectTo<ProductModel>(_configMapper).OrderByDescending(x=>x.ViewTime).Take(10).ToListAsync();
            return data;
        }

        public async Task<object> GetSoldViewProductTop10ByPartnerId(int partnerId)
        {
            var data = await _productRepository.FindAll(x => x.SoldView > 0 && x.PartnerId == partnerId)
            .ProjectTo<ProductModel>(_configMapper).OrderByDescending(x => x.ViewTime).Take(10).ToListAsync();
            return data;
        }
    }
}
