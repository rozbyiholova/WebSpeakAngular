using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL
{
    public static class ConfigurateOptions
    {
        public static DbContextOptions<ProductHouseContext> GetOptions()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            string connectionString = @"Server=(LocalDb)\MSSQLLocalDB;Database=ProductHouse;Trusted_Connection=True;ConnectRetryCount=0";

            var optionsBuilder = new DbContextOptionsBuilder<ProductHouseContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }
    }
}
