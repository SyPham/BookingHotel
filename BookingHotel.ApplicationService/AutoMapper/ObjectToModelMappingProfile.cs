using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.ApplicationService.AutoMapper
{
    public class ObjectToModelMappingProfile : Profile
    {
        public ObjectToModelMappingProfile()
        {
            CreateMap<Faq, FaqModel>();
            CreateMap<Account, AccountModel>().ForMember(d => d.AccountTypeName, o => o.MapFrom(s => s.AccountType != null ? s.AccountType.Title : ""));
            CreateMap<AccountType, AccountTypeModel>();
            CreateMap<CustomerType, CustomerTypeModel>();
            CreateMap<Customer, CustomerModel>();
            CreateMap<ArticleCategory, ArticleCategoryModel>()
                  .ForMember(d => d.ChildTotal, o => o.MapFrom(s => s.Articles != null ? s.Articles.Count() : 0))
                  .ForMember(d => d.ParentName, o => o.MapFrom(s => s.ParentId != null ? s.Title : ""));
            CreateMap<ServiceCategory, ServiceCategoryModel>()
                  .ForMember(d => d.ParentName, o => o.MapFrom(s => s.ParentId != null ? s.Title : ""));
            CreateMap<ProductCategory, ProductCategoryModel>()
                  .ForMember(d => d.ParentName, o => o.MapFrom(s => s.ParentId != null ? s.Title : ""));
            CreateMap<ProductCategory, MainMenuModel>();
            CreateMap<Article, ArticleModel>().ForMember(d => d.ArticleCategoryName, o => o.MapFrom(s => s.ArticleCategory != null ? s.ArticleCategory.Title : ""));
            CreateMap<Service, ServiceModel>().ForMember(d => d.ServiceCategoryName, o => o.MapFrom(s => s.ServiceCategory != null ? s.ServiceCategory.Title : "")); 
            CreateMap<Product, ProductModel>()
                .ForMember(d => d.ObjPartner, o => o.MapFrom(s => s.Partner))
                .ForMember(d => d.PartnerName, o => o.MapFrom(s => s.Partner != null ? s.Partner.Title : ""))
                .ForMember(d => d.MethodUsedName, o => o.MapFrom(s => s.MethodUsed != null ? s.MethodUsed.Title : ""))
                .ForMember(d => d.ProductCategoryName, o => o.MapFrom(s => s.ProductCategory != null ? s.ProductCategory.Title : ""));
            CreateMap<ProductTab, ProductTabModel>();
            CreateMap<ProductWishlist, ProductWishlistModel>();
            CreateMap<Banner, BannerModel>();
            CreateMap<District, DistrictModel>();
            CreateMap<Province, ProvinceModel>();
            CreateMap<Partner, PartnerModel>().ForMember(d => d.PartnerTypeName, o => o.MapFrom(s => s.PartnerType != null ? s.PartnerType.Title : ""));
            CreateMap<PartnerType, PartnerTypeModel>();
            CreateMap<PartnerLocal, PartnerLocalModel>();
            CreateMap<Tab, TabModel>();
            CreateMap<Feedback, FeedbackModel>();
            CreateMap<Recruitment, RecruitmentModel>();
            CreateMap<Contact, ContactModel>();
            CreateMap<Page, PageModel>();
            CreateMap<PageType, PageTypeModel>();
            CreateMap<Sale, SaleModel>();
            CreateMap<Assess, AssessModel>();
            CreateMap<Assess, AssessWebModel>();
            CreateMap<PayType, PayTypeModel>();
            CreateMap<Unit, UnitModel>();
            CreateMap<Order, OrderModel>();
            CreateMap<OrderDetail, OrderDetailModel>();
            CreateMap<OrderStatus, OrderStatusModel>();
            CreateMap<NotificationType, NotificationTypeModel>();
            CreateMap<Notification, NotificationModel>()
                  .ForMember(d => d.NotificationTypeName, o => o.MapFrom(s => s.NotificationType != null ? s.NotificationType.Title : ""));
            CreateMap<ProductWallet, ProductWalletModel>();
            CreateMap<Cart, CartModel>();
            CreateMap<NotificationAccount, NotificationAccountModel>();
            CreateMap<TabType, TabTypeModel>();
            CreateMap<MethodUsed, MethodUsedModel>();
            //
            // Lite
            CreateMap<Account, AccountLiteModel>();
            CreateMap<Customer, CustomerLiteModel>();
            CreateMap<Cart, CartLiteModel>();
            CreateMap<NotificationAccount, NotificationAccountLiteModel>();
            CreateMap<Order, OrderLiteModel>();
            CreateMap<Order, OrderLite2Model>();
            CreateMap<OrderDetail, OrderDetailLiteModel>();
            CreateMap<ProductWishlist, ProductWishlistLiteModel>();
            //
            CreateMap<GroupFunction, GroupFunctionModel>();
            CreateMap<Module, ModuleModel>();
            CreateMap<About, AboutModel>();
            CreateMap<Function, FunctionModel>().ForMember(d => d.ModuleName, o => o.MapFrom(s => s.Module != null ? s.Module.Title : ""));
            CreateMap<ProductCategory, ProductCategoryHotModel>().ForMember(d => d.Products, o => o.MapFrom(s => s.Products.Where(x => x.Status == true && x.EffectiveDate <= DateTime.UtcNow && x.ExpiryDate >= DateTime.UtcNow).Take(3).ToList()));
            CreateMap<GroupFunction, GroupFunctionModel>();
            CreateMap<Product, ProductByCatModel>();

        }
    }
}
