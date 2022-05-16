using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using BookingHotel.Data.Entities;

namespace BookingHotel.Model.Auth
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JwtToken { get; set; }
        public bool IsVerified { get; set; }
        public string Avatar { get; set; }
        public object Permissions{ get; set; }
        public object Menus{ get; set; }
        
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string MessageType { get; set; }

        public AuthenticateResponse(Account user, string fullName, string jwtToken, string refreshToken)
        {
            Id = user.Id.ToString();
            UserName = user.UserName;
            FullName = fullName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            IsVerified = user.IsVerified;
        }
        public AuthenticateResponse(Account user, string fullName, string jwtToken, string refreshToken, string email, string phone, string avatar)
        {
            Id = user.Id.ToString();
            UserName = user.UserName;
            FullName = fullName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            Email = email;
            Phone = phone;
            Avatar = avatar;
            IsVerified = user.IsVerified;
        }

        public AuthenticateResponse()
        {
        }
    }
}
