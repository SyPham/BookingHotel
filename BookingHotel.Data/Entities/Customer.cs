using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerPartners = new HashSet<CustomerPartner>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public string Birthday { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? NumberTakeCare { get; set; }
        public string CompanyName { get; set; }
        public int? AccountId { get; set; }
        public int? Point { get; set; }
        public string ReferralCode { get; set; }
        public string Code { get; set; }

        public virtual Account Account { get; set; }
        public virtual CustomerType CustomerType { get; set; }
        public virtual ICollection<CustomerPartner> CustomerPartners { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
