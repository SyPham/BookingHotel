using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Lite
{
   public class OrderLite2Model
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobi { get; set; }
        public string Address { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? SaleOff { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool? IsPoint { get; set; }
        public int? Point { get; set; }
    }
}
