
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace AMS.Test
{
    class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.Implementing.AMSContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], r => r.UseRowNumberForPaging()));


            services.AddScoped<Data.Interface.IDbAccessor, Data.Implementing.DbAccessor>();
            services.AddScoped<Services.Customers.IAccountService, Services.Customers.AccountService>();
        }
    }
}
