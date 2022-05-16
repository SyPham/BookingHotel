using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.Data.Entities;

namespace BookingHotel.Model.Catalog
{
    public class PartnerModel
    {
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
        public string Banner { get; set; }
        public int? ViewTime { get; set; }
        public int? TotalAssess { get; set; }
        public string PartnerTypeName { get; set; }
        public string Email { get; set; }
        public string Representative { get; set; }
        public string Phone { get; set; }
        public decimal? ValueAssess { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

        public virtual AccountModel Account { get; set; }
        public virtual ICollection<PartnerLocal> PartnerLocals { get; set; }

        public List<IFormFile> FilesAvatar { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesThumb { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesBanner { get; set; } = new List<IFormFile>();
    }
}
