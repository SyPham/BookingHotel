using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class ProductTab
    {
        public int TabId { get; set; }
        public int ProductId { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }

        public virtual Product Product { get; set; }
        public virtual Tab Tab { get; set; }
    }
}
