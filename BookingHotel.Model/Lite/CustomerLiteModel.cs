using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Lite
{
    public class CustomerLiteModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public string Birthday { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        //public int? NumberTakeCare { get; set; }
        public string CompanyName { get; set; }
        public string ReferralCode { get; set; }
        //public string Code { get; set; }

        public IFormFile File { get; set; }

        public virtual AccountLiteModel objAccount { get; set; }
    }
}
