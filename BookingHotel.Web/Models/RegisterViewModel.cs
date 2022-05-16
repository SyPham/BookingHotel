using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingHotel.Web.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
     
    }
    public class RegisterPartnerViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        public int? PartnerTypeId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Representative { get; set; }
    }
    public class RegisterCustomerViewModel
    {
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
