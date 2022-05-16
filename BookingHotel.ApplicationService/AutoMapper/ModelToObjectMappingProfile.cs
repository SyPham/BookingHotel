using AutoMapper;
using BookingHotel.Data.Entities;
using BookingHotel.Model.Catalog;
using BookingHotel.Model.Lite;

namespace BookingHotel.ApplicationService.AutoMapper
{
    public class ModelToObjectMappingProfile : Profile
    {
        public ModelToObjectMappingProfile()
        {
            // new class
            CreateMap<FaqModel, Faq>();
            CreateMap<ArticleCategoryModel, ArticleCategory>();
            CreateMap<ServiceCategoryModel, ServiceCategory>();
            CreateMap<ProductCategoryModel, ProductCategory>();
            CreateMap<ArticleModel, Article>();
            CreateMap<ServiceModel, Service>();
            CreateMap<ProductModel, Product>();
            CreateMap<ProductTabModel, ProductTab>();
            CreateMap<ProductWishlistModel, ProductWishlist>();
            CreateMap<BannerModel, Banner>();
            CreateMap<DistrictModel, District>();
            CreateMap<ProvinceModel, Province>();
            CreateMap<AccountTypeModel, AccountType>();
            CreateMap<AccountModel, Account>();
            CreateMap<CustomerTypeModel, CustomerType>();
            CreateMap<CustomerModel, Customer>();
            CreateMap<PartnerModel, Partner>();
            CreateMap<PartnerTypeModel, PartnerType>();
            CreateMap<PartnerLocalModel, PartnerLocal>();
            CreateMap<TabModel, Tab>();
            CreateMap<FeedbackModel, Feedback>();
            CreateMap<RecruitmentModel, Recruitment>();
            CreateMap<ContactModel, Contact>();
            CreateMap<PageModel, Page>();
            CreateMap<PageTypeModel, PageType>();
            CreateMap<SaleModel, Sale>();
            CreateMap<AssessModel, Assess>();
            CreateMap<PayTypeModel, PayType>();
            CreateMap<UnitModel, Unit>();
            CreateMap<OrderModel, Order>();
            CreateMap<OrderDetailModel, OrderDetail>();
            CreateMap<OrderStatusModel, OrderStatus>();
            CreateMap<ProductWalletModel, ProductWallet>();
            CreateMap<NotificationTypeModel, NotificationType>();
            CreateMap<NotificationModel, Notification>();
            CreateMap<CartModel, Cart>();
            CreateMap<NotificationAccountModel, NotificationAccount>();
            CreateMap<TabTypeModel, TabType>();
            CreateMap<MethodUsedModel, MethodUsed>();
            //
            // Lite
            CreateMap<AccountLiteModel, Account>();
            CreateMap<CustomerLiteModel, Customer>();
            CreateMap<CartLiteModel, Cart>();
            CreateMap<NotificationAccountLiteModel, NotificationAccount>();
            CreateMap<OrderLiteModel, Order>();
            CreateMap<OrderLite2Model, Order>();
            CreateMap<OrderDetailLiteModel, OrderDetail>();
            CreateMap<ProductWishlistLiteModel, ProductWishlist>();
            //
            CreateMap<GroupFunctionModel, GroupFunction>();
            CreateMap<ModuleModel, Module>();
            CreateMap<AboutModel, About>();
            CreateMap<FunctionModel, Function>();
            CreateMap<GroupFunctionModel, GroupFunction>();

            CreateMap<ProductActionModel, Product>()
             .ForMember(d => d.IsSale, o => o.Ignore())
             .ForMember(d => d.Price, o => o.Ignore())
             .ForMember(d => d.IsSell, o => o.Ignore());
            CreateMap<ProductCategoryHotModel, ProductCategory>();
            CreateMap<AssessWebModel, Assess>();
            CreateMap<MainMenuModel, ProductCategory>();

        }
    }
}
