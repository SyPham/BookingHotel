using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingHotel.Model.Auth
{
   public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
