using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.Api.Installers;
using BookingHotel.ApplicationService.AutoMapper;
using BookingHotel.Common.Helpers;
using BookingHotel.Data.Entities;

namespace BookingHotel.Api
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
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
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
            SpaStartup.ConfigureServices(services);
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VietCouponDBContext vietCouponContext)
        {
            //vietCouponContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(KspSpecificOrigins);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Web Build");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            SpaStartup.Configure(app);
        }
    }
}
