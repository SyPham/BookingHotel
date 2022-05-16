using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BookingHotel.Data.Entities;

namespace BookingHotel.Data.EF
{
    public class VietCouponDBContextFactory : IDesignTimeDbContextFactory<VietCouponDBContext>
    {
        public VietCouponDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectString = configuration.GetConnectionString("DBConnection");

            var optionsBuilder = new DbContextOptionsBuilder<VietCouponDBContext>();

            optionsBuilder.UseSqlServer(connectString);
            return new VietCouponDBContext(optionsBuilder.Options);
        }
    }
}
