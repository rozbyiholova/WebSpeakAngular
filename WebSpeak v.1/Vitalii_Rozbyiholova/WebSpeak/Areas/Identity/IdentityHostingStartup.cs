using System;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSpeak.Models;

[assembly: HostingStartup(typeof(WebSpeak.Areas.Identity.IdentityHostingStartup))]
namespace WebSpeak.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<ProductHouseContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentityContextConnection")));

                services.AddDefaultIdentity<Users>(options => { options.ClaimsIdentity.UserIdClaimType = "UserID"; })
                    .AddEntityFrameworkStores<ProductHouseContext>();
            });
        }
    }
}