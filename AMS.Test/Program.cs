
using AMS.Services.Customers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AMS.Test
{
    class Program
    {
        static IServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            // BuildWebHost(args).Run();
            Init();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
               
                IAccountService accountService = serviceProvider.GetService<IAccountService>();
                var query = accountService.GetRoleList().ToList();
                Console.WriteLine(query.FirstOrDefault() == null ? "null" : query.FirstOrDefault().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception:" + ex.Message);
            }
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("sw总共花费{0}ms.", ts2.TotalMilliseconds);
            Console.ReadKey();
        }

        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            IConfiguration Configuration = builder.Build();
            IServiceCollection services = new ServiceCollection();
           // services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<Data.Implementing.AMSContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], r => r.UseRowNumberForPaging()));


            services.AddScoped<Data.Interface.IDbAccessor, Data.Implementing.DbAccessor>();
            services.AddScoped<Services.Customers.IAccountService, Services.Customers.AccountService>();
            serviceProvider = services.BuildServiceProvider();
        }

    }

}
