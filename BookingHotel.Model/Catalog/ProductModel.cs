using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class ProductModel
    {
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
        public decimal? Price => IsSale ? (OriginalPrice * (100 - Sale)) / 100 : OriginalPrice;
        public decimal? OriginalPrice { get; set; }
        public decimal? OldPrice { get; set; }
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
        public string ProductCategoryName { get; set; }
        public string PartnerName { get; set; }
        public string MethodUsedName { get; set; }
        public int? PartnerId { get; set; }
        public int? SaleId { get; set; }
        public int? UnitId { get; set; }
        public int? MethodUsedId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsSell => EffectiveDate <= DateTime.UtcNow && ExpiryDate >= DateTime.UtcNow;
        public int? Point { get; set; }
        public bool IsWishlist { get; set; }
        public List<TabRequest> TabRequest { get; set; } = new List<TabRequest>();
        public List<AssessModel> AssessRequest { get; set; }
        public virtual PartnerModel ObjPartner { get; set; }
        public virtual ProductCategoryModel ProductCategory { get; set; }
        public List<IFormFile> FilesAvatar { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesThumb { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesImageList { get; set; } = new List<IFormFile>();
    }
    public class ProductActionModel
    {
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
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public int? Sale { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleDeadLine { get; set; }
        public bool IsSale { get; set; }
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
        public string ProductCategoryName { get; set; }
        public string PartnerName { get; set; }
        public string MethodUsedName { get; set; }
        public int? PartnerId { get; set; }
        public int? SaleId { get; set; }
        public int? UnitId { get; set; }
        public int? MethodUsedId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsSell { get; set; }
        public int? Point { get; set; }
        public bool IsWishlist { get; set; }
        public List<TabRequest> TabRequest { get; set; } = new List<TabRequest>();
        public List<AssessModel> AssessRequest { get; set; }
        public virtual PartnerModel ObjPartner { get; set; }
        public List<IFormFile> FilesAvatar { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesThumb { get; set; } = new List<IFormFile>();
        public List<IFormFile> FilesImageList { get; set; } = new List<IFormFile>();
    }
    public class TabRequest
    {
        public int? TabTypeId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public int TabId { get; set; }
    }
    public class ProductByCatModel
    {
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
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public int? Sale { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleDeadLine { get; set; }
        public bool IsSale { get; set; }
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
        public string ProductCategoryName { get; set; }
        public string PartnerName { get; set; }
        public string MethodUsedName { get; set; }
        public int? PartnerId { get; set; }
        public int? SaleId { get; set; }
        public int? UnitId { get; set; }
        public int? MethodUsedId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsSell { get; set; }
        public int? Point { get; set; }
        public bool IsWishlist { get; set; }
        public ProductCategoryModel ProductCategory { get; set; }
 
    }
    public class FilterRequest
    {
        public List<int> catIds { get; set; }
        public bool Percent { get; set; }
        public bool Increase { get; set; }
        public bool Decrease { get; set; }
        public List<int> PartnerIds { get; set; }
        public List<int> Rate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public class ValueAssessRequest
    {
        public decimal? ValueAssess { get; set; }
        public int TotalAssess { get; set; }
        public int Id { get; set; }

    }
}
