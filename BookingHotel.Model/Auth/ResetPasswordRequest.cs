using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingHotel.Model.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
