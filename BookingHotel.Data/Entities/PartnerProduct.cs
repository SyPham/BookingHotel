using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class PartnerProduct
    {
        public int PartnerId { get; set; }
        public int ProductId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual Partner Partner { get; set; }
        public virtual Product Product { get; set; }
    }
}
