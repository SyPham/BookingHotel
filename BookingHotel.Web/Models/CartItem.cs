using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Web.Models
{
    public class CartItem
    {
        public int Quantity { set; get; }
        public ProductModel Product { set; get; }
    }
}
