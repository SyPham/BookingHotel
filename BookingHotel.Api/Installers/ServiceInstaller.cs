using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Data.Entities;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.Common.Helpers;
using BookingHotel.ApplicationRepository.UnitOfWork;
using BookingHotel.ApplicationService.Interface;
using BookingHotel.ApplicationService.Implementation;
using BookingHotel.ApplicationService.Implementation.Mobile;

namespace BookingHotel.Api.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connetionString = configuration.GetConnectionString("DBConnection");
            services.AddDbContext<VietCouponDBContext>(options =>
                options.UseSqlServer(
                    connetionString,
                    o => o.MigrationsAssembly("BookingHotel.Data")));

            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerTypeService, CustomerTypeService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
            services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IPartnerTypeService, PartnerTypeService>();
            services.AddScoped<IPartnerLocalService, PartnerLocalService>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IRecruitmentService, RecruitmentService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IPageTypeService, PageTypeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<ITabService, TabService>();
            services.AddScoped<IProductTabService, ProductTabService>();
            services.AddScoped<IProductWishlistService, ProductWishlistService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IAssessService, AssessService>();
            services.AddScoped<IPayTypeService, PayTypeService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<INotificationAccountService, NotificationAccountService>();
            services.AddScoped<ITabTypeService, TabTypeService>();
            services.AddScoped<IMethodUsedService, MethodUsedService>();
            //
            //Lite
            services.AddScoped<IAccountLiteService, AccountLiteService>();
            services.AddScoped<ICustomerLiteService, CustomerLiteService>();
            services.AddScoped<ICartLiteService, CartLiteService>();
            services.AddScoped<INotificationAccountLiteService, NotificationAccountLiteService>();
            services.AddScoped<IOrderLiteService, OrderLiteService>();
            services.AddScoped<IOrderDetailLiteService, OrderDetailLiteService>();
            services.AddScoped<IProductWishlistLiteService, ProductWishlistLiteService>();
            //
            services.AddScoped<IProductWalletService, ProductWalletService>();
            services.AddScoped<IGroupFunctionService, GroupFunctionService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IAboutService, AboutService>();
            services.AddScoped<IAuthService, AuthService>();
            //
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILogging, Logging>();
            services.AddScoped<IWebHelper, WebHelper>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFunctionService, FunctionService>();
            services.AddScoped<IGroupFunctionService, GroupFunctionService>();
            services.AddScoped<IMobileBaseService, MobileBaseService>();
        }
    }
}
