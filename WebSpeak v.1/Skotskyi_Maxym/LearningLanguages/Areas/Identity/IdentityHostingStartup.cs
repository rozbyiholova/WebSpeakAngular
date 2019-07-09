using System;

using LearningLanguages.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DAL.Models;

[assembly: HostingStartup(typeof(LearningLanguages.Areas.Identity.IdentityHostingStartup))]
namespace LearningLanguages.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Languages_bdContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentityContextConnection")));

                services.AddDefaultIdentity<Users>()
                    .AddEntityFrameworkStores<Languages_bdContext>();
            });
        }
    }
}