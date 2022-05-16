using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.Model.Lite;

namespace BookingHotel.Model.Catalog
{
    public class ProductWalletModel
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

        public virtual AccountModel Account { get; set; }
        public virtual OrderLite2Model Order { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual MethodUsedModel MethodUsed { get; set; }
    }
}
