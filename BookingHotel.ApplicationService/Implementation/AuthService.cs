using AutoMapper;
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

namespace BookingHotel.ApplicationService.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IBaseRepository<Customer> _customerRepository;
        private readonly IBaseRepository<Partner> _partnerRepository;
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly ILogging _logging;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        private ProcessingResult processingResult;
        public AuthService(
            IMapper mapper,
            MapperConfiguration configMapper,
            ILogging logging,
            IOptions<AppSettings> appSettings,
            IAccountRepository accountRepository,
            IBaseRepository<Customer> customerRepository,
            IBaseRepository<Partner> partnerRepository,
            IAccountService accountService,
            IEmailService emailService,
            IUnitOfWork unitOfWork
            )
        {
            _appSettings = appSettings.Value;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _partnerRepository = partnerRepository;
            _accountService = accountService;
            _emailService = emailService;
            _logging = logging;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var account = await _accountRepository.FindAll()
               .Include(x => x.RefreshTokens)
               .Include(x => x.AccountType)
               .FirstOrDefaultAsync(x => x.UserName == model.Username && x.Password == model.Password);

            // return null if user not found
            if (account == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(account);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            account.RefreshTokens.Add(refreshToken);
            _accountRepository.Update(account);
            await _unitOfWork.SaveAll();
            var data = new AuthenticateResponse(account, "", jwtToken, refreshToken.Token);
            data.Permissions = await _accountService.GetActionFunctionByAccountId(account.Id);
            //data.Menus = await _accountService.GetMenusByAccountId(account.Id);

            processingResult = new ProcessingResult() {
                MessageType = MessageTypeEnum.Success, 
                Message = "", Success = true,
                Data = data
        };
            return processingResult;
        }

        public async Task<ProcessingResult> Register(RegisterRequest request)
        {
            var item = new Account {
                UserName = request.Username,
                Password= request.Password,
                Status = true,
                CreateTime = DateTime.Now,
                AccountTypeId = 1 };

            try
            {
                if (await _accountRepository.FindAll(x => x.UserName == request.Username).AnyAsync())
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
            }
            return processingResult;
        }
        public async Task<ProcessingResult> RegisterCustomer(RegisterCustomerRequest request)
        {
            var item = new Account
            {
                UserName = request.Username,
                Password = request.Password,
                Status = true,
                CreateTime = DateTime.Now,
                AccountTypeId = 2
            };

            try
            {
                if (await _accountRepository.FindAll(x => x.UserName == request.Username).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Tài khoản đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }
                if (await _customerRepository.FindAll(x => x.Email == request.Email).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Email đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }
                if (await _customerRepository.FindAll(x => x.Phone == request.Phone).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Số điện thoại đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }
                _accountRepository.Add(item);
                await _unitOfWork.SaveAll();

                var customer = new Customer
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    CustomerTypeId = request.CustomerTypeId ?? 1,
                    ReferralCode = request.ReferralCode,
                    AccountId = item.Id
                };
                _customerRepository.Add(customer);
                await _unitOfWork.SaveAll();

                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "", Success = false };
            }
            return processingResult;
        }
        public async Task<ProcessingResult> RegisterPartner(RegisterPartnerRequest request)
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
                if (await _accountRepository.FindAll(x => x.UserName == request.Username).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Tài khoản đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }
                if (await _partnerRepository.FindAll(x => x.Email == request.Email).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Email đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }
                if (await _partnerRepository.FindAll(x => x.Phone == request.Phone).AnyAsync())
                {
                    processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = "Số điện thoại đang được sử dụng!", Success = false, Data = item };
                    return processingResult;
                }
                _accountRepository.Add(item);

                await _unitOfWork.SaveAll();
                var partner = new Partner
                {
                    Title = request.Title,
                    Email = request.Email,
                    PartnerTypeId = request.PartnerTypeId,
                    AccountId = item.Id,
                    Representative = request.Representative
                };
                _partnerRepository.Add(partner);
                await _unitOfWork.SaveAll();
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Success, Message = "", Success = true, Data = item };
            }
            catch (Exception ex)
            {
                processingResult = new ProcessingResult() { MessageType = MessageTypeEnum.Danger, Message = ex.Message, Success = false };
            }
            return processingResult;
        }



        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var account = await _accountRepository.FindAll().Include(x=>x.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

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

            // generate new jwt
            var jwtToken = generateJwtToken(account);
            var data = new AuthenticateResponse(account, "", jwtToken, newRefreshToken.Token);
            data.Permissions = await _accountService.GetActionFunctionByAccountId(account.Id);
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
                Expires = DateTime.UtcNow.AddMinutes(15),
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
                    Expires = DateTime.Now.AddDays(7),
                    CreateTime = DateTime.Now,
                    CreatedByIp = ipAddress
                };
            }
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


        private void sendVerificationEmail(Account account, string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/verify-email?token={account.VerificationToken}";
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
                message = $@"<p>Nếu bạn không biết mật khẩu của mình, vui lòng truy cập <a href=""{origin}/auth/forgot-password"">Quên mật khẩu</a>.</p>";
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
        private string randomTokenString(int maxlength = 5)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, maxlength).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}
