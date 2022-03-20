// This Startup file is based on ASP.NET Core new project templates and is included
// as a starting point for DI registration and HTTP request processing pipeline configuration.
// This file will need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core startup files, see https://docs.microsoft.com/aspnet/core/fundamentals/startup

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using T034.Core;
using T034.Core.Api;
using T034.Core.DataAccess;
using T034.Core.Services;
using T034.Tools.IO;

namespace T034
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
            services.AddControllersWithViews(ConfigureMvcOptions)
                // Newtonsoft.Json is added for compatibility reasons
                // The recommended approach is to use System.Text.Json for serialization
                // Visit the following link for more guidance about moving away from Newtonsoft.Json to System.Text.Json
                // https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to
                .AddNewtonsoftJson(options =>
                {
                    options.UseMemberCasing();
                });

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Content")),
                RequestPath = new PathString("/Content")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Scripts")),
                RequestPath = new PathString("/Scripts")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"fonts")),
                RequestPath = new PathString("/fonts")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), Configuration["ImagesFolder"])),
                RequestPath = new PathString("/" + Configuration["ImagesFolder"])
            });
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        { 
        }


        /// <summary>
        /// Регистрация сервисов
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private void RegisterServices(IServiceCollection services)
        {
            var dbFilePath = Configuration.GetConnectionString("DatabaseFile");

            string filesFolder = Configuration["FilesFolder"];

            services.AddTransient(sp =>
            {
                var webHostEnvironment = sp.GetService<IWebHostEnvironment>();
                var path = webHostEnvironment.ContentRootPath;

                var str = $"Data Source={path.Replace("\\", "/")}/{dbFilePath};Version=3;";
                return new NhDbFactory(str).CreateBaseDb();
            });

            services.AddTransient<Repository.IRepository, Repository.Repository>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFileService>(sp =>
            {
                var webHostEnvironment = sp.GetService<IWebHostEnvironment>();
                var contentRootPath = webHostEnvironment.ContentRootPath;

                var path = Path.Combine(contentRootPath, filesFolder);
                return new FileService(sp.GetService<IBaseDb>(), path);
            });

            services.AddTransient<Core.Services.Administration.IUserService, Core.Services.Administration.UserService>();
            services.AddTransient<Core.Services.Administration.IRoleService, Core.Services.Administration.RoleService>();
            services.AddTransient<IMenuItemService, MenuItemService>();
            services.AddTransient<INewslineService, NewslineService>();

            services.AddTransient<IFileUploader>(sp =>
            {
                var webHostEnvironment = sp.GetService<IWebHostEnvironment>();
                var contentRootPath = webHostEnvironment.ContentRootPath;

                var path = Path.Combine(contentRootPath, filesFolder);
                return new FileUploader(path);
            });

            string imagesFolder = Configuration["ImagesFolder"];
            services.AddTransient<IImageUploader>(sp =>
            {
                var webHostEnvironment = sp.GetService<IWebHostEnvironment>();
                return new ImageUploader(webHostEnvironment, imagesFolder);
            });
        }
    }
}
