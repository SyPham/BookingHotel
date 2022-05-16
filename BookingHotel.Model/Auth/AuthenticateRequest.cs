using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingHotel.Model.Auth
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class AuthenticateRequestWeb
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public int AccountTypeId { get; set; }
    }
    public class AuthenticatePartnerRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
