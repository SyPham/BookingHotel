using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingHotel.Web.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int AccountTypeId { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
