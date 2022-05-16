using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class CartModel
    {
        public CartModel()
        {
        }

        public CartModel(int accountId, int productId, int? quantity, DateTime createTime, ProductModel product)
        {
            AccountId = accountId;
            ProductId = productId;
            Quantity = quantity;
            CreateTime = createTime;
            Product = product;
        }

        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual ProductModel Product { get; set; }
    }
   
}
