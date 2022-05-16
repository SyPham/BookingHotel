using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Web.Models
{
    public class MenuModel
    {
        public MenuModel(List<ProductCategoryModel> productCategory, List<ServiceCategoryModel> serviceCategory, List<ArticleCategoryModel> articleCategory, AboutModel about)
        {
            ProductCategory = productCategory;
            ServiceCategory = serviceCategory;
            ArticleCategory = articleCategory;
            About = about;
        }

        public List<ProductCategoryModel> ProductCategory { get; set; }
        public List<ServiceCategoryModel> ServiceCategory { get; set; }
        public List<ArticleCategoryModel> ArticleCategory { get; set; }
        public AboutModel About { get; set; }
    }
}
