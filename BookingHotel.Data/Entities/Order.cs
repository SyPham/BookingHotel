using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductWallets = new HashSet<ProductWallet>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public decimal? TotalPrice { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobi { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public bool? IsDelete { get; set; }
        public string Remark { get; set; }
        public int? PayTypeId { get; set; }
        public int? CustomerId { get; set; }
        public string NoteSale { get; set; }
        public decimal? SaleOff { get; set; }
        public string NoteFeeShip { get; set; }
        public int? OrderStatusId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? IsPoint { get; set; }
        public int? Point { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual PayType PayType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductWallet> ProductWallets { get; set; }
    }
}
