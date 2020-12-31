using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Repository;
using BookStore.Repository.DbSeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<DatabaseContext>(o =>
            {
                o.UseSqlServer(Configuration["ConnectionStrings:BookStoreConnection"]);
            });

            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddRazorPages();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<Cart>(x => SessionCart.GetCart(x));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseEndpoints(ep => {
                ep.MapControllerRoute("genrepage", "{genre:int}/Page{bookPage:int}",
                    new { Controller = "Home", action = "Index" });

                ep.MapControllerRoute("page", "Page{bookPage:int}",
                    new { Controller = "Home", action = "Index", bookPage = 1 });

                ep.MapControllerRoute("genre", "{genre}",
                    new { Controller = "Home", action = "Index", bookPage = 1 });

                ep.MapControllerRoute("pagination", "Books/{bookPage:int}",
                    new { Controller = "Home", action = "Index", bookPage = 1 });

                ep.MapDefaultControllerRoute();
                ep.MapRazorPages();
                ep.MapBlazorHub();

                ep.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
            });

            SeedData.EnsureDataExists(app);
        }
    }
}