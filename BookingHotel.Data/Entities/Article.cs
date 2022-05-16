using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Thumb { get; set; }
        public string Content { get; set; }
        public string SourcePage { get; set; }
        public string SourceLink { get; set; }
        public int? Position { get; set; }
        public bool? Status { get; set; }
        public int? ViewTime { get; set; }
        public int? TotalAssess { get; set; }
        public decimal? ValueAssess { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Schemas { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int? ArticleCategoryId { get; set; }

        public virtual ArticleCategory ArticleCategory { get; set; }
    }
}
