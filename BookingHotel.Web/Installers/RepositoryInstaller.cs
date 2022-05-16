using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.Loggings;
using BookingHotel.ApplicationRepository.BaseRepository;
using BookingHotel.ApplicationRepository.Implementation;
using BookingHotel.ApplicationRepository.Interface;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;

namespace BookingHotel.Web.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connetionString = configuration.GetConnectionString("DBConnection");
            services.AddDbContext<VietCouponDBContext>(options =>
                options.UseSqlServer(
                    connetionString,
                    o => o.MigrationsAssembly("BookingHotel.Data")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IPartnerTypeRepository, PartnerTypeRepository>();
            services.AddScoped<IPartnerLocalRepository, PartnerLocalRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<ITabRepository, TabRepository>();
            services.AddScoped<IProductTabRepository, ProductTabRepository>();
            services.AddScoped<IProductWishlistRepository, ProductWishlistRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            //
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
            services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IRecruitmentRepository, RecruitmentRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageTypeRepository, PageTypeRepository>();
            services.AddScoped<IPayTypeRepository, PayTypeRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IAssessRepository, AssessRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            services.AddScoped<IProductWalletRepository, ProductWalletRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<INotificationAccountRepository, NotificationAccountRepository>();
            services.AddScoped<ITabTypeRepository, TabTypeRepository>();
            services.AddScoped<IMethodUsedRepository, MethodUsedRepository>();
            //
            services.AddScoped<IGroupFunctionRepository, GroupFunctionRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<ILogging, Logging>();
            services.AddScoped<IWebHelper, WebHelper>();
        }
    }
}
