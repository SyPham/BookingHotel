using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class CustomerModel
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
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public int? NumberTakeCare { get; set; }
        public string CompanyName { get; set; }
        public int? AccountId { get; set; }
        public int? Point { get; set; }
        public string ReferralCode { get; set; }
        public string Code { get; set; }
        //public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesAvatar { get; set; } = new List<IFormFile>();
        public virtual AccountModel Account { get; set; }
    }
    public class CustomerInfoModel
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
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public int? NumberTakeCare { get; set; }
        public string CompanyName { get; set; }
        public int? AccountId { get; set; }
        public int? Point { get; set; }
        public string ReferralCode { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }

        public virtual AccountModel Account { get; set; }
    }
    public class UploadAvatarRequest
    {
        public IFormFile File{ get; set; }
        public int AccountId { get; set; }

    }
    public class UpdateInfoRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
