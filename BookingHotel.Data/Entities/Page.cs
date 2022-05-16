using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public int? Position { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Schemas { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int? PageTypeId { get; set; }
        public string Url { get; set; }
        public bool? IsUrl { get; set; }
        public string Alias { get; set; }

        public virtual Account CreateByNavigation { get; set; }
        public virtual PageType PageType { get; set; }
    }
}
