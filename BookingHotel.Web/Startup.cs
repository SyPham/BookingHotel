using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.AutoMapper;
using BookingHotel.Common.Helpers;
using BookingHotel.Web.ClassHelpers;
using BookingHotel.Web.Installers;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace BookingHotel.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private readonly string KspSpecificOrigins = "KspSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var config = Configuration.GetSection("Config").Get<Config>();
            services.Configure<Config>(Configuration.GetSection("Config"));
            services.AddOptions();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();
            services.AddScoped<IMapper>(sp =>
            {
                return new Mapper(AutoMapperConfig.RegisterMappings());
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton(AutoMapperConfig.RegisterMappings());
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.InstallServicesInAssembly(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(KspSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(appSettings.AllowOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }
            );

            // configure jwt authentication

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(options => {
                options.DefaultScheme= CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                })
             .AddCookie()
             .AddFacebook(facebookOptions =>
             {
                 facebookOptions.AppId = appSettings.FacebookAppId;
                 facebookOptions.AppSecret = appSettings.FacebookAppSecret;
                 facebookOptions.AccessDeniedPath = "/dang-nhap";
                 facebookOptions.CallbackPath = "/dang-nhap-facebook";
             });
            // services.AddAuthentication(x =>
            // {
            //     x.DefaultAuthenticateScheme = "VietcouponsSecurityScheme";
            // })
            //.AddCookie("VietcouponsSecurityScheme", options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Account/Access");
            //    options.Cookie = new CookieBuilder
            //    {
            //        //Domain = "",
            //        HttpOnly = true,
            //        Name = ".aspNetCoreVietcoupons.Security.Cookie",
            //        Path = "/",
            //        SameSite = SameSiteMode.Lax,
            //        SecurePolicy = CookieSecurePolicy.SameAsRequest
            //    };
            //    options.Events = new CookieAuthenticationEvents
            //    {
            //        OnSignedIn = context =>
            //        {
            //            Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
            //                "OnSignedIn", context.Principal.Identity.Name);
            //            return Task.CompletedTask;
            //        },
            //        OnSigningOut = context =>
            //        {
            //            Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
            //                "OnSigningOut", context.HttpContext.User.Identity.Name);
            //            return Task.CompletedTask;
            //        },
            //        OnValidatePrincipal = context =>
            //        {
            //            Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
            //                "OnValidatePrincipal", context.Principal.Identity.Name);
            //            return Task.CompletedTask;
            //        }
            //    };
            //    //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            //    options.LoginPath = new PathString("/Account/Login");
            //    options.ReturnUrlParameter = "RequestPath";
            //    options.SlidingExpiration = true;
            //});

            services.AddHttpClient("default", client =>
            {
                client.BaseAddress = new Uri(config.ApiBase);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                // access the DI container
                var serviceProvider = services.BuildServiceProvider();
                // Find the HttpContextAccessor service
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                // Get the bearer token from the request context (header)
                //var bearerToken = httpContextAccessor.HttpContext.Request
                //                      .Headers["Authorization"]
                //                      .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));

                //// Add authorization if found
                //if (bearerToken != null)
                //    client.DefaultRequestHeaders.Add("Authorization", bearerToken);

                // Other settings
                var jwtToken = httpContextAccessor.HttpContext.User.FindFirst("JwtToken");
                if (jwtToken != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken.Value);

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Web Build", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                     {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                    }
                });
            });
            services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            services.AddSession(cfg =>
            {                    // Đăng ký dịch vụ Session
                cfg.Cookie.Name = "vietcoupon";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 30, 0);    // Thời gian tồn tại của Session
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseCors(KspSpecificOrigins);
            app.UseSwagger();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Web Build");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //   name: "article-list",
                //   pattern: "/tin-tuc/{id:int}/{*title}",
                //   defaults: new { controller = "Article", action = "Index", id = 0 }
                //   );

                //endpoints.MapControllerRoute(
                //  name: "article-detail",
                //  pattern: "/chi-tiet-tin-tuc/{id:int}/{*title}",
                //  defaults: new { controller = "Article", action = "Detail", id = 0 }
                //  );

                //endpoints.MapControllerRoute(
                // name: "article-list-all",
                // pattern: "/tin-tuc",
                // defaults: new { controller = "Article", action = "Index" }
                // );

                //endpoints.MapControllerRoute(
                //  name: "gallery-list-all",
                //  pattern: "/hinh-anh",
                //  defaults: new { controller = "Gallery", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                //   name: "gallery-list",
                //   pattern: "/hinh-anh/{id:int}/{*title}",
                //   defaults: new { controller = "Gallery", action = "Index" }
                //   );

                //endpoints.MapControllerRoute(
                //  name: "video-list-all",
                //  pattern: "/video",
                //  defaults: new { controller = "Gallery", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                //  name: "video-list",
                //  pattern: "/video/{id:int}/{*title}",
                //  defaults: new { controller = "Gallery", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                //   name: "event-list",
                //   pattern: "/su-kien",
                //   defaults: new { controller = "Event", action = "Index" }
                //   );

                //endpoints.MapControllerRoute(
                //   name: "event-detail",
                //   pattern: "/chi-tiet-su-kien",
                //   defaults: new { controller = "Event", action = "Detail" }
                //   );

                //endpoints.MapControllerRoute(
                //  name: "donation-list",
                //  pattern: "/quyen-gop",
                //  defaults: new { controller = "Donation", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                //  name: "donation-detail",
                //  pattern: "/chi-tiet-quyen-gop/{id:int}/{*title}",
                //  defaults: new { controller = "Donation", action = "Detail", id = 0 }
                //  );

                //endpoints.MapControllerRoute(
                //  name: "contact",
                //  pattern: "/lien-he",
                //  defaults: new { controller = "Contact", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                //  name: "about",
                //  pattern: "/gioi-thieu",
                //  defaults: new { controller = "About", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                //  name: "register",
                //  pattern: "/dang-nhap",
                //  defaults: new { controller = "MyAccount", action = "Index" }
                //  );

                //endpoints.MapControllerRoute(
                // name: "userinfo",
                // pattern: "/thong-tin-ca-nhan",
                // defaults: new { controller = "MyAccount", action = "UserInfo" }
                // );

                //endpoints.MapControllerRoute(
                // name: "search-detail",
                // pattern: "/tim-kiem/{keywork:string}",
                // defaults: new { controller = "Search", action = "Index", id = 0 }
                // );
            });
            //SpaStartup.Configure(app);
        }
    }
}
