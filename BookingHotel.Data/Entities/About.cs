using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class About
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Thumb { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public string Hotline { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Zalo { get; set; }
        public string Blog { get; set; }
        public string Youtube { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Pinterest { get; set; }
        public string Linkedin { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Logo { get; set; }

        public virtual Account CreateByNavigation { get; set; }
    }
}
