using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EFCore;

namespace ShopApp.WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddScoped<IProductRepository, EFCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EFCoreCategoryRepository>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions{
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                    RequestPath="/modules"
            });
            
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"adminproducts",
                    pattern:"admin/products",
                    defaults: new {controller="Admin", action="ProductList"}
                );
                endpoints.MapControllerRoute(
                    name:"adminproductcreate",
                    pattern:"admin/products/create",
                    defaults: new {controller="Admin", action="ProductCreate"}
                );
                endpoints.MapControllerRoute(
                    name:"adminproductedit",
                    pattern:"admin/products/{id?}",
                    defaults: new {controller="Admin", action="ProductEdit"}
                );
                endpoints.MapControllerRoute(
                    name:"admincategories",
                    pattern:"admin/categories",
                    defaults: new {controller="Admin", action="CategoryList"}
                );
                endpoints.MapControllerRoute(
                    name:"admincategorycreate",
                    pattern:"admin/categories/create",
                    defaults: new {controller="Admin", action="CategoryCreate"}
                );
                endpoints.MapControllerRoute(
                    name:"admincategoryedit",
                    pattern:"admin/categories/{id?}",
                    defaults: new {controller="Admin", action="CategoryEdit"}
                );
                endpoints.MapControllerRoute(
                    name:"search",
                    pattern:"search",
                    defaults: new {controller="Shop", action="Search"}
                );
                endpoints.MapControllerRoute(
                    name:"productdetails",
                    pattern:"{url}",
                    defaults: new {controller="Shop", action="Details"}
                );
                endpoints.MapControllerRoute(
                    name:"products",
                    pattern:"products/{category?}",
                    defaults: new {controller="Shop", action="List"}
                );
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
