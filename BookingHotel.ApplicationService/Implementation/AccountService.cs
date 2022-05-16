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
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.Common;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly AppSettings _appSettings;
        private readonly ILogging _logging;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBaseRepository<AccountFunction> _accountFunctionRepository;
        private readonly IBaseRepository<Assess> _assessRepository;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
        private readonly IBaseRepository<Partner> _partnerRepository;
        private readonly IBaseRepository<Function> _functionRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private ProcessingResult processingResult;

        public AccountService(
            IMapper mapper,
            MapperConfiguration configMapper,
            IOptions<AppSettings> appSettings,
            ILogging logging,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IBaseRepository<AccountFunction> accountFunctionRepository,
            IBaseRepository<Assess> assessRepository,
            IBaseRepository<RefreshToken> refreshTokenRepository,
            IBaseRepository<Partner> partnerRepository,
            IBaseRepository<Function> _functionRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _appSettings = appSettings.Value;
            _logging = logging;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _accountFunctionRepository = accountFunctionRepository;
            _assessRepository = assessRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _partnerRepository = partnerRepository;
            this._functionRepository = _functionRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> AddAsync(AccountModel model)
        {
            var item = _mapper.Map<Account>(model);
            try
            {
                if (await _accountRepository.FindAll(x=>x.UserName == model.UserName).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Tài khoản đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }

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

        public async Task<ProcessingResult> DeleteAsync(int id)
        {
            var item = await _accountRepository.FindAll()
                .Include(x=>x.RefreshTokens)
                .Include(x=>x.AccountFunctions)
                .Include(x=>x.Customers)
                .Include(x=>x.Assesses)
                .Include(x=>x.Carts)
                .Include(x=>x.ProductWishlists)
                .Include(x=>x.NotificationAccounts)
                .FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if (item.UserName == "admin")
                {
                    return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Không được xóa tài khoản admin", Success = false };

                }
                item.Carts = null;
                item.ProductWishlists = null;
                item.NotificationAccounts = null;
                _refreshTokenRepository.RemoveMultiple(item.RefreshTokens.ToList());
                _assessRepository.RemoveMultiple(item.Assesses.ToList());
                _customerRepository.RemoveMultiple(item.Customers.ToList());
                _accountFunctionRepository.RemoveMultiple(item.AccountFunctions.ToList());
                _accountRepository.Remove(item);
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

        public async Task<AccountModel> FindById(int id)
        {
            var item = await _accountRepository.FindAll().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<AccountModel>(item);
        }

        public async Task<AccountModel> GetByUsername(string username)
        {
            var item = await _accountRepository.FindAll().FirstOrDefaultAsync(x => x.UserName == username);
            return _mapper.Map<AccountModel>(item);
        }

        public async Task<List<AccountModel>> GetAllAsync()
        {
            return await _accountRepository.FindAll().Include(x => x.AccountType).Where(x => x.AccountType.Title != "Partner").ProjectTo<AccountModel>(_configMapper).ToListAsync();
        }

        public async Task<Pager> PaginationAsync(ParamaterPagination paramater)
        {
            var query = _accountRepository.FindAll().ProjectTo<AccountModel>(_configMapper);
            return await query.ToPaginationAsync(paramater.page, paramater.pageSize);
        }

        public async Task<ProcessingResult> UpdateAsync(AccountModel model)
        {
            var item = await _accountRepository.FindAll(x => x.Id == model.Id).AsNoTracking().FirstOrDefaultAsync();
            if (item == null )
            {
                return processingResult;

            }
            if (await _accountRepository.FindAll(x => x.UserName == model.UserName).AnyAsync())
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Tài khoản đang được sử dụng!", Success = false, Data = item };
                return processingResult;
            }
            try
            {
                item.UserName = model.UserName;
                item.AccountTypeId = model.AccountTypeId;
                item.AcceptTerms = model.AcceptTerms;
                item.Status = model.Status; 
                item.Password = model.Password;
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

        public async Task<ProcessingResult> ChangePassword(ChangePasswordRequest model)
        {
            var item = await _accountRepository.FindAll(x => x.Password == model.OldPassword && x.Id == model.Id).AsNoTracking().FirstOrDefaultAsync();
            if (item == null)
                return new ProcessingResult { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };

            try
            {
                item.Password = model.NewPassword;
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

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress, int accountTypeId)
        {
            // return null if user not found
            var user = _accountRepository.FindAll().Include(x => x.RefreshTokens).SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password && x.AccountTypeId == accountTypeId);
            if (user == null) return null;

            var customer = _customerRepository.FindAll().SingleOrDefault(x => x.AccountId == user.Id);
            if (customer == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            user.RefreshTokens.Add(refreshToken);
            _accountRepository.Update(user);
            _unitOfWork.Save();

            return new AuthenticateResponse(user, customer.FullName, jwtToken, refreshToken.Token);
        }
        public AuthenticateResponse AuthenticateWeb(AuthenticateRequestWeb model, string ipAddress)
        {
            // khach hang 
            if (model.AccountTypeId == 2)
            {
                // return null if user not found
                var user = _accountRepository.FindAll().Include(x => x.RefreshTokens).SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password && x.AccountTypeId == model.AccountTypeId);
                if (user == null) return null;

                var customer = _customerRepository.FindAll().SingleOrDefault(x => x.AccountId == user.Id);
                if (customer == null) return null;

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(user);
                var refreshToken = generateRefreshToken(ipAddress);

                // save refresh token
                user.RefreshTokens.Add(refreshToken);
                _accountRepository.Update(user);
                _unitOfWork.Save();
                return new AuthenticateResponse(user, customer.FullName, jwtToken, refreshToken.Token, customer.Email, customer.Phone, customer.Avatar);

            }
            // Doi tac
            if (model.AccountTypeId == 3)
            {
                // return null if user not found
                var user = _accountRepository.FindAll().Include(x => x.RefreshTokens).SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password && x.AccountTypeId == model.AccountTypeId);
                if (user == null) return null;

                var partner = _partnerRepository.FindAll().SingleOrDefault(x => x.AccountId == user.Id);
                if (partner == null) return null;

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(user);
                var refreshToken = generateRefreshToken(ipAddress);

                // save refresh token
                user.RefreshTokens.Add(refreshToken);
                _accountRepository.Update(user);
                _unitOfWork.Save();
                return new AuthenticateResponse(user, partner.Representative, jwtToken, refreshToken.Token, partner.Email, partner.Phone, partner.Avatar);
            }

            return new AuthenticateResponse();
        }
        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _accountRepository.FindAll().Include(x => x.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null) return null;

            var customer = _customerRepository.FindById(user.Customers.FirstOrDefault().Id);
            if (customer == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.RevokeTime = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _accountRepository.Update(user);
            _unitOfWork.Save();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, customer.FullName, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _accountRepository.FindAll().Include(x => x.RefreshTokens).SingleOrDefault(x => x.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.RevokeTime = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _accountRepository.Update(user);
            _unitOfWork.Save();

            return true;
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

        public async Task<ProcessingResult> ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var customer = _customerRepository.FindAll().SingleOrDefault(x => x.Email == model.Email);
            if (customer == null) return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Thông tin không tồn tại", Success = false };

            var account = _accountRepository.FindAll().SingleOrDefault(x => x.Id == customer.AccountId);
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

        public async Task<ProcessingResult> ValidateResetToken(ValidateResetTokenRequest model)
        {
            try
            {
                var account = await _accountRepository.FindAll().SingleOrDefaultAsync(x =>
                    x.ResetToken == model.Token &&
                    x.ResetTokenExpiresTime > DateTime.UtcNow);

                if (account == null)
                    return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Xác thực thất bại", Success = false };

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "Xác thực không thành công", Success = true, Data = account };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                _logging.LogException(ex, model);
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

        private string generateJwtToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
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

        private void sendVerificationEmail(Account account, string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
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
                message = $@"<p>Nếu bạn không biết mật khẩu của mình, vui lòng truy cập <a href=""{origin}/account/forgot-password"">Quên mật khẩu</a>.</p>";
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
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
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

        public async Task<List<AccountModel>> GetAllByAccountTypeID(int accountTypeId)
        {
            if (accountTypeId == 0)
                return await GetAllAsync();
            return await _accountRepository.FindAll(x => x.AccountTypeId == accountTypeId).Include(x => x.AccountType).ProjectTo<AccountModel>(_configMapper).ToListAsync();

        }

        public async Task<ProcessingResult> Lock(int id)
        {
            var item = _accountRepository.FindById(id);
            try
            {
                item.Status = !item.Status;
                _accountRepository.Update(item);
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
        public async Task<ProcessingResult> PostAccountFunction(PostAccountFunctionRequest request)
        {
            var model = _mapper.Map<Account>(request.Account);
            _accountRepository.Add(model);
            await _unitOfWork.SaveAll();
            var newAccountFunction = new List<AccountFunction>();
            var parentIds = await _functionRepository.FindAll(x => request.FunctionIds.Contains(x.Id)).Where(x => x.ParentId != null).Select(x => x.ParentId.Value).Distinct().ToListAsync();
            var ids = parentIds.Union(request.FunctionIds).ToList();
            foreach (var functionSystemId in ids)
            {
                newAccountFunction.Add(new AccountFunction(model.Id, functionSystemId));
            }
            var existingAccountFunction = _accountFunctionRepository.FindAll().Where(x => x.AccountId == model.Id).ToList();
            _accountFunctionRepository.RemoveMultiple(existingAccountFunction);
            _accountFunctionRepository.AddRange(newAccountFunction.DistinctBy(x => new { x.AccountId, x.FunctionId }).ToList());
            try
            {
                var result = await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = request };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Save function package failed", Success = false };
                _logging.LogException(ex, request);
            }
            return processingResult;
        }

        public async Task<ProcessingResult> PutAccountFunction(PutAccountFunctionRequest request)
        {
            try
            {
                var model = _mapper.Map<Account>(request.Account);

            _accountRepository.Update(model);
            await _unitOfWork.SaveAll();
            var newAccountFunction = new List<AccountFunction>();
                var parentIds2 = await _functionRepository.FindAll().ToListAsync();
            var parentIds = await _functionRepository.FindAll(x => request.FunctionIds.Contains(x.Id)).Where(x => x.ParentId.HasValue).Select(x => x.ParentId.Value).Distinct().ToListAsync();
                var ids = parentIds.Union(request.FunctionIds).ToList();
            foreach (var functionSystemId in ids)
            {
                newAccountFunction.Add(new AccountFunction(model.Id, functionSystemId));
            }

            var existingAccountFunction = _accountFunctionRepository.FindAll().Where(x => x.AccountId == model.Id).ToList();
            _accountFunctionRepository.RemoveMultiple(existingAccountFunction);
            _accountFunctionRepository.AddRange(newAccountFunction.DistinctBy(x => new { x.AccountId, x.FunctionId }).ToList());
           
                var result = await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = request };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Save group function failed", Success = false };
                _logging.LogException(ex, request);
            }
            return processingResult;
        }
        public async Task<object> GetFunctionsAccount(int accountId)
        {
            if (accountId == 0)
            {
                var query = await _functionRepository.FindAll(x=>x.Status)
                                   .Select(a => new
                                   {
                                       a.Id,
                                       a.Title,
                                       Module = a.Module.Title,
                                       a.ParentId,
                                       a.Position,
                                       IsChecked = false,

                                   }).ToListAsync();
                //var data = query.AsHierarchy(x => x.Id, y => y.ParentId).ToList();
                var groupBy = query.GroupBy(g => g.Module)
                    .Select(a => new
                    {
                        Module = a.Key,
                        Field = new
                        {
                            DataSource = a.AsHierarchy(x => x.Id, y => y.ParentId).OrderBy(x => x.Entity.Position).Select(s => new
                            {
                                Id = s.Entity.Id,
                                Name = s.Entity.Title,
                                ParentId = s.Entity.ParentId,
                                s.Entity.IsChecked,
                                Child = s.ChildNodes.OrderBy(x => x.Entity.Position).Select(f => new {
                                    Id = f.Entity.Id,
                                    Name = f.Entity.Title,
                                    ParentId = f.Entity.ParentId,
                                    f.Entity.IsChecked
                                })
                            }),
                            Id = "id",
                            Text = "name",
                            ParentId = "parentId",
                            hasChildren = "hasChild"
                        }
                    });
                return groupBy;
            }
            else
            {
                var query = await (from a in _functionRepository.FindAll(x=>x.Status).Include(x => x.Module)
                                   from fp in _accountFunctionRepository.FindAll().Where(x => x.AccountId == accountId && a.Id == x.FunctionId && x.Function.Status).DefaultIfEmpty()
                                   select new
                                   {
                                       a.Id,
                                       a.Title,
                                       a.Position,
                                       Module = a.Module.Title,
                                       a.ParentId,
                                       IsChecked = fp != null,
                                   }).ToListAsync();
                var groupBy = query.GroupBy(g => g.Module)
                         .Select(a => new
                         {
                             Module = a.Key,
                             Field = new
                             {
                                 DataSource = a.AsHierarchy(x => x.Id, y => y.ParentId).OrderBy(x=>x.Entity.Position).Select(s => new
                                 {
                                     Id = s.Entity.Id,
                                     Name = s.Entity.Title,
                                     ParentId = s.Entity.ParentId,
                                     s.Entity.IsChecked,
                                     Child = s.ChildNodes.OrderBy(x => x.Entity.Position).Select(f=> new {
                                         Id = f.Entity.Id,
                                         Name = f.Entity.Title,
                                         ParentId = f.Entity.ParentId,
                                         f.Entity.IsChecked
                                     })
                                 }).ToList(),
                                 Id = "id",
                                 Text = "name",
                                 ParentId = "parentId",
                                 hasChildren = "hasChild"
                             }
                         });
                return groupBy;
            }
        }

        public async Task<object> GetActionFunctionByAccountId(int accountId)
        {
            return await _accountFunctionRepository.FindAll(x => x.AccountId == accountId && x.Function.Status)
                .Include(x => x.Function)
                .Select(x => new
                {
                    x.Function.Action,
                    x.Function.Controller,
                    x.Function.Url,
                    x.Function.Title
                })
                .ToListAsync();
        }

        public async Task<object> GetMenusByAccountId(int accountId)
        {
            var data = await _accountFunctionRepository.FindAll(x => x.AccountId == accountId && x.Function.Status)
                .Include(x => x.Function)
                .Select(x => new
                {
                    x.FunctionId,
                    x.Function.ParentId,
                    Module = x.Function.Module,
                    x.Function.Action,
                    x.Function.Position,
                    x.Function.Controller,
                    x.Function.Url,
                    x.Function.Icon,
                    x.Function.Title
                })
                .ToListAsync();
            var groupBy = data.GroupBy(x => x.Module).Select(x => new
            {
                Module = x.Key,
                Data = x.AsHierarchy(x => x.FunctionId, y => y.ParentId).OrderBy(x => x.Entity.Position).ToList()

            }).ToList();
            return groupBy;
        }

        public async Task<ProcessingResult> DeleteRangeAsync(List<int> ids)
        {
            FileExtension fileExtension = new FileExtension();

            var query = await _accountRepository.FindAll(x => ids.Contains(x.Id))
                 .Include(x => x.RefreshTokens)
                 .Include(x => x.AccountFunctions)
                 .Include(x => x.Customers)
                 .Include(x => x.Assesses)
                 .Include(x => x.Carts)
                 .Include(x => x.ProductWishlists)
                 .Include(x => x.NotificationAccounts)
                .ToListAsync();
            foreach (var item in query)
            {
                try
                {
                    if (item.UserName == "admin")
                    {
                        return new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Không được xóa tài khoản admin", Success = false };

                    }
                    _refreshTokenRepository.RemoveMultiple(item.RefreshTokens.ToList());
                    _assessRepository.RemoveMultiple(item.Assesses.ToList());
                    _customerRepository.RemoveMultiple(item.Customers.ToList());
                    _accountFunctionRepository.RemoveMultiple(item.AccountFunctions.ToList());
                    item.Carts = null;
                    item.ProductWishlists = null;
                    item.NotificationAccounts = null;
                    _accountRepository.Remove(item);
                    await _unitOfWork.SaveAll();
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
                }
                catch (Exception ex)
                {
                   
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
                    _logging.LogException(ex, new { id = ids });
                    return processingResult;
                }
            }
            return processingResult;
        }

    }
}
