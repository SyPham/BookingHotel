using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingHotel.Model.Auth
{
    public class TokenVerifyEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
