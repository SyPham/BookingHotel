using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class RecruitmentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Thumb { get; set; }
        public string Content { get; set; }
        public int? ViewTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }
        public bool? Status { get; set; }
        public int? Position { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Schemas { get; set; }
        public bool? IsDelete { get; set; }
    }
}
