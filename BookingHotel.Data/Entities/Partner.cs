using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Partner
    {
        public Partner()
        {
            CustomerPartners = new HashSet<CustomerPartner>();
            PartnerLocals = new HashSet<PartnerLocal>();
            PartnerProducts = new HashSet<PartnerProduct>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public bool? Status { get; set; }
        public int? Position { get; set; }
        public string Description { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }
        public int? PartnerTypeId { get; set; }
        public int? AccountId { get; set; }
        public string Thumb { get; set; }
        public string Content { get; set; }
        public string Representative { get; set; }
        public string Banner { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? ViewTime { get; set; }
        public int? TotalAssess { get; set; }
        public decimal? ValueAssess { get; set; }

        public virtual Account CreateByNavigation { get; set; }
        public virtual PartnerType PartnerType { get; set; }
        public virtual ICollection<CustomerPartner> CustomerPartners { get; set; }
        public virtual ICollection<PartnerLocal> PartnerLocals { get; set; }
        public virtual ICollection<PartnerProduct> PartnerProducts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
