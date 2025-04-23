using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitmqWeb.ExcelCreate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitmqWeb.ExcelCreate
{
    public class Program
    {
        public static void Main(string[] args)
        {
           CreateHostBuilder(args).Build().Run();

            //using (var scope=host.Services.CreateScope())
            //{
            //    var appdbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    var usermanager=scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //    if (!appdbcontext.Users.Any()) 
            //    {
            //        usermanager.CreateAsync(new IdentityUser()
            //        {
            //            UserName = "deneme",
            //            Email = "deneme@gmail.com"
            //        }, "password12"
            //        ).Wait();
            //        usermanager.CreateAsync(new IdentityUser()
            //        {
            //            UserName = "deneme2",
            //            Email = "deneme2@gmail.com"
            //        }, "password12"
            //        ).Wait();
            //    }
            //    appdbcontext.Database.Migrate();

            //}
            //host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
