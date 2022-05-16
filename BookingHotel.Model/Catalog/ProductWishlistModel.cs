using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class ProductWishlistModel
    {
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }
}
