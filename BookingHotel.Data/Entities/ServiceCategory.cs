using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class ServiceCategory
    {
        public ServiceCategory()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int? Position { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Schemas { get; set; }
        public DateTime CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }
        public int? ParentId { get; set; }
        public string Alias { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Account CreateByNavigation { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
