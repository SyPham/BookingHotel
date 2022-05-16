using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingHotel.Api
{
    internal class SpaStartup
    {
        internal static void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = @"wwwroot\AdminApp";
                //if (env.IsDevelopment())
                //{
                //    spa.UseAngularCliServer(npmScript: "start");
                //}
            });
            // https://stackoverflow.com/questions/48216929/how-to-configure-asp-net-core-server-routing-for-multiple-spas-hosted-with-spase
            //app.Map("/admin", admin =>
            //{
            //    admin.UseSpa(spa =>
            //    {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            //spa.Options.SourcePath = @"wwwroot\AdminApp";

            //spa.UseSpaPrerendering(options =>
            //{
            //    options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.bundle.js";
            //    options.BootModuleBuilder = env.IsDevelopment()
            //        ? new AngularCliBuilder(npmScript: "build:ssr:en")
            //        : null;
            //    options.ExcludeUrls = new[] { "/sockjs-node" };
            //    options.SupplyData = (context, data) =>
            //    {
            //        data["foo"] = "bar";
            //    };
            //});

            //if (env.IsDevelopment())
            //{
            //    spa.UseAngularCliServer(npmScript: "start --app=admin --base-href=/admin/ --serve-path=/");
            //}
            //    });
            //});
            // here you can see we make sure it doesn't start with /api, if it does, it'll 404 within .NET if it can't be found

        }

        internal static void ConfigureServices(IServiceCollection services)
        {
            //added me
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = @"wwwroot\AdminApp";
            });
        }
    }
}
