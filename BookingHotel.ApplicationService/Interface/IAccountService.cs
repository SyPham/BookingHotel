using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Auth;

namespace BookingHotel.ApplicationService.Interface
{
    public interface IAccountService : IBaseService<AccountModel>
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress, int accountTypeId);
        AuthenticateResponse AuthenticateWeb(AuthenticateRequestWeb model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        Task<AccountModel> GetByUsername(string username);
        bool RevokeToken(string token, string ipAddress);
        Task<ProcessingResult> SendTokenVerifyEmail(int accountId, string email, string origin);
        Task<ProcessingResult> VerifyEmail(string token);
        Task<ProcessingResult> ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<ProcessingResult> ValidateResetToken(ValidateResetTokenRequest model);
        Task<ProcessingResult> ResetPassword(ResetPasswordRequest model);
        Task<ProcessingResult> ChangePassword(ChangePasswordRequest request);
        Task<List<AccountModel>> GetAllByAccountTypeID(int accountTypeId);
        Task<ProcessingResult> Lock(int id);
        Task<ProcessingResult> PostAccountFunction(PostAccountFunctionRequest request);
        Task<ProcessingResult> PutAccountFunction(PutAccountFunctionRequest request);
        Task<object> GetFunctionsAccount(int accountId);
        Task<object> GetActionFunctionByAccountId(int accountId);
        Task<object> GetMenusByAccountId(int accountId);
        Task<ProcessingResult> DeleteRangeAsync(List<int> ids);
    }
}
