using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.Data.Entities;

namespace BookingHotel.Model.Catalog
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
        public bool AcceptTerms { get; set; }
        //public Role Role { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiresTime { get; set; }
        public DateTime? PasswordResetTime { get; set; }
        public bool IsVerified => VerifiedTime.HasValue || PasswordResetTime.HasValue;
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
    public class PostAccountFunctionRequest
    {
        public AccountModel Account { get; set; }
        public List<int> FunctionIds { get; set; } = new List<int>();
    }
    public class PutAccountFunctionRequest
    {
        public AccountModel Account { get; set; }
        public List<int> FunctionIds { get; set; } = new List<int>();
    }
}
