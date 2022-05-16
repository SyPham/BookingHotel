using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class ProductWishlist
    {
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }
    }
}
