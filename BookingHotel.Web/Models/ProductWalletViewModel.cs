using System.Collections.Generic;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Web.Models
{
    public class ProductWalletViewModel
    {
        public ProductWalletViewModel(List<ProductModel> productAll, List<ProductModel> productIsUse, List<ProductModel> productNotUse)
        {
            ProductAll = productAll;
            ProductIsUse = productIsUse;
            ProductNotUse = productNotUse;
        }

        public List<ProductModel> ProductAll { get; set; }
        public List<ProductModel> ProductIsUse { get; set; }
        public List<ProductModel> ProductNotUse { get; set; }
    }
}
