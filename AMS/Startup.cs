﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //注入配置
            // services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddMvc();
            //数据库
            services.AddDbContext<Data.Implementing.AMSContext>(options =>
                   options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], r => r.UseRowNumberForPaging()));
            //cookie
            services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            //service
            services.AddScoped<Data.Interface.IDbAccessor, Data.Implementing.DbAccessor>();
            services.AddScoped<Services.Customers.IAccountService, Services.Customers.AccountService>();
            services.AddScoped<Services.Authentication.IAuthenticationService, Services.Authentication.FormsAuthenticationService>();
            //定时任务
            //services.AddTimedJob();
            //session
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
