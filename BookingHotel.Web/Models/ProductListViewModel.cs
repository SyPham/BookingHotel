using System.Collections.Generic;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Web.Models
{
    public class ProductListViewModel
    {
        public ProductListViewModel(List<ProductModel> productAll, List<ProductModel> productIsSale, List<ProductModel> productStop)
        {
            ProductAll = productAll;
            ProductIsSale = productIsSale;
            ProductStop = productStop;
        }

        public List<ProductModel> ProductAll { get; set; }
        public List<ProductModel> ProductIsSale { get; set; }
        public List<ProductModel> ProductStop { get; set; }
    }
}
