using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    class ConfigurateOptions
    {
        public static DbContextOptions<Languages_bdContext> GetOptions()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            string connectionString = @"Server=VINW0231;Database=Languages_bd;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<Languages_bdContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }
    }
}
