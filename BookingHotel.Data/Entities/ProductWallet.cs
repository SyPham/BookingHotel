using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class ProductWallet
    {
        public int AccountId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public bool? IsAssess { get; set; }
        public bool? IsUse { get; set; }
        public DateTime? UseTime { get; set; }
        public string OriginKey { get; set; }
        public int? OriginValue { get; set; }
        public DateTime CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }
        public int? MethodUsedId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual MethodUsed MethodUsed { get; set; }
    }
}
