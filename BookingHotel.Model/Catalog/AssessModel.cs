using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class AssessModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public decimal? NumberStar { get; set; }
        public int? KeyId { get; set; }
        public string Phone { get; set; }
        public string KeyName { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? AccountId { get; set; }

        public virtual ProductModel ProductRequest { get; set; }
    }
    public class AssessWebModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public decimal? NumberStar { get; set; }
        public int? KeyId { get; set; }
        public string KeyName { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? AccountId { get; set; }

        public virtual ProductModel ProductRequest { get; set; }
        public string CustomerAvatar { get; set; }
        public string PartnerAvatar { get; set; }

    }
}
