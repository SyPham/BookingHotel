using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PartnerProducts = new HashSet<PartnerProduct>();
            ProductWallets = new HashSet<ProductWallet>();
            ProductTabs = new HashSet<ProductTab>();
            ProductWishlists = new HashSet<ProductWishlist>();
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
        public string Thumb { get; set; }
        public string ImageListProduct { get; set; }
        public int? Position { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Alias { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? Price => IsSale ? (OriginalPrice * (100 - Sale)) / 100 : OriginalPrice;
        public decimal? OriginalPrice { get; set; }
        public int? Sale { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleDeadLine { get; set; }
        public bool IsSale => Sale > 0 && SaleStart <= DateTime.UtcNow && SaleDeadLine >= DateTime.UtcNow;
        public int? ViewTime { get; set; }
        public bool? StockStatus { get; set; }
        public int? WishlistView { get; set; }
        public int? SoldView { get; set; }
        public string Schemas { get; set; }
        public int TotalAssess { get; set; }
        public decimal? ValueAssess { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? CreateBy { get; set; }
        public int? ModifyBy { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? PartnerId { get; set; }
        public int? SaleId { get; set; }
        public int? UnitId { get; set; }
        public int? MethodUsedId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsSell => EffectiveDate <= DateTime.UtcNow && ExpiryDate >= DateTime.UtcNow;
        public int? Point { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual Sale SaleNavigation { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual MethodUsed MethodUsed { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PartnerProduct> PartnerProducts { get; set; }
        public virtual ICollection<ProductWallet> ProductWallets { get; set; }
        public virtual ICollection<ProductTab> ProductTabs { get; set; }
        public virtual ICollection<ProductWishlist> ProductWishlists { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
