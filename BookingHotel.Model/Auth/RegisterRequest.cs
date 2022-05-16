using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingHotel.Model.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class RegisterPartnerRequest
    {
        public RegisterPartnerRequest(string username, string password, string phone, int? partnerTypeId, string title, string email, string representative)
        {
            Username = username;
            Password = password;
            Phone = phone;
            PartnerTypeId = partnerTypeId;
            Title = title;
            Email = email;
            Representative = representative;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]

        public string Phone { get; set; }
        public int? PartnerTypeId { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        [Required]
        public string Representative { get; set; }
    }
    public class RegisterCustomerRequest
    {
        public RegisterCustomerRequest(string username, string password, string phone, int? customerTypeId, string referralCode, string email, string fullName)
        {
            Username = username;
            Password = password;
            Phone = phone;
            CustomerTypeId = customerTypeId;
            ReferralCode = referralCode;
            Email = email;
            FullName = fullName;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]

        public string Phone { get; set; }
        public int? CustomerTypeId { get; set; }
        public string ReferralCode { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }

    }
}
