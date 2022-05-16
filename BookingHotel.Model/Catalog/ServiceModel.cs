using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Thumb { get; set; }
        public string Content { get; set; }
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
        public int? ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public string Alias { get; set; }
        public bool? IsDelete { get; set; }
        public List<IFormFile> FilesAvatar { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesThumb { get; set; } = new List<IFormFile>();
    }
}
