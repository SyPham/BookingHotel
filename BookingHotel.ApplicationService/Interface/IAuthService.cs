using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.Common;
using BookingHotel.Model.Auth;

namespace BookingHotel.ApplicationService.Interface
{
   public interface IAuthService
    {

        Task<ProcessingResult> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        Task<bool> RevokeToken(string token, string ipAddress);
        Task<ProcessingResult> Register(RegisterRequest request);
        Task<ProcessingResult> RegisterCustomer(RegisterCustomerRequest request);
        Task<ProcessingResult> RegisterPartner(RegisterPartnerRequest request);

        Task<ProcessingResult> SendTokenVerifyEmail(int accountId, string email, string origin);
        Task<ProcessingResult> VerifyEmail(string token);
    }
}
