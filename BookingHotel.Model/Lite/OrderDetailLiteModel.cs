using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Lite
{
    public class OrderDetailLiteModel
    {
        //public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? Point { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string Option { get; set; }
    }
}
