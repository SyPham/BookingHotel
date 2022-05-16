using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Lite
{
   public class OrderLiteModel
    {
        //public int Id { get; set; }
        public string Code { get; set; }
        public decimal? TotalPrice { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobi { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string NoteSale { get; set; }
        public decimal? SaleOff { get; set; }
        public string NoteFeeShip { get; set; }
        public int? OrderStatusId { get; set; }
        public int? PayTypeId { get; set; }
        public bool? IsPoint { get; set; }
        public int? Point { get; set; }

        public virtual ICollection<OrderDetailLiteModel> OrderDetails { get; set; }
    }
}
