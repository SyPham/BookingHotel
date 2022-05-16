using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingHotel.Model.Auth
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
