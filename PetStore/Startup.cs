using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Models;
using PetStore.Repository;
using PetStore.Repository.DbSeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PetStore
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
                o.UseSqlServer(Configuration["ConnectionStrings:PetStoreConnection"]);
            });

            services.AddDbContext<IdentityContext>(o =>
            {
                o.UseSqlServer(Configuration["ConnectionStrings:PetStoreIdentityConnection"]);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

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
            if (env.IsProduction())
            {
                app.UseExceptionHandler("/error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(ep => {
                ep.MapControllerRoute("ArticleTypepage", "{ArticleType:int}/Page{ArticlePage:int}",
                    new { Controller = "Home", action = "Index" });

                ep.MapControllerRoute("page", "Page{ArticlePage:int}",
                    new { Controller = "Home", action = "Index", ArticlePage = 1 });

                ep.MapControllerRoute("ArticleType", "{ArticleType}",
                    new { Controller = "Home", action = "Index", ArticlePage = 1 });

                ep.MapControllerRoute("pagination", "Articles/{ArticlePage:int}",
                    new { Controller = "Home", action = "Index", ArticlePage = 1 });

                ep.MapDefaultControllerRoute();
                ep.MapRazorPages();
                ep.MapBlazorHub();

                ep.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
            });

            SeedData.EnsureDataExists(app);

            SeedIdentityData.EnsurePopulated(app);
        }
    }
}