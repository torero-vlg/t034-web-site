// This application entry point is based on ASP.NET Core new project templates and is included
// as a starting point for app host configuration.
// This file may need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core hosting, see https://docs.microsoft.com/aspnet/core/fundamentals/host/web-host

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ninject;
using Ninject.Web.AspNetCore;
using Ninject.Web.AspNetCore.Hosting;
using Ninject.Web.Common;
using Ninject.Web.Common.SelfHost;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using T034.Core;
using T034.Core.Api;
using T034.Core.DataAccess;
using T034.Core.Services;
using T034.Models;
using T034.Repository;
using T034.Tools.Attribute;

namespace T034
{
    public class Program
    {
        /// <summary>
        /// Соответствие метода-контроллера и роли
        /// </summary>
        public static IEnumerable<ActionRole> ActionRoles { get; private set; }
        
        public static string FilesFolder = ConfigurationManager.AppSettings["FilesFolder"];

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            var hostConfiguration = new AspNetCoreHostConfiguration(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .BlockOnStart();

            var host = new NinjectSelfHostBootstrapper(CreateKernel, hostConfiguration);
            host.Start();

            ActionRoles = GetActionRoles();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static IKernel CreateKernel()
        {
            var settings = new NinjectSettings();
            // Unfortunately, in .NET Core projects, referenced NuGet assemblies are not copied to the output directory
            // in a normal build which means that the automatic extension loading does not work _reliably_ and it is
            // much more reasonable to not rely on that and load everything explicitly.
            settings.LoadExtensions = false;

            var kernel = new AspNetCoreKernel(settings);

            RegisterServices(kernel);


            kernel.Load(typeof(AspNetCoreHostConfiguration).Assembly);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IBaseDb>().ToMethod(c => new NhDbFactory(ConnectionString).CreateBaseDb());
            kernel.Bind<IRepository>().To<Repository.Repository>().InRequestScope();
            kernel.Bind<ISettingService>().To<SettingService>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<IFileService>().To<FileService>().InRequestScope();

            kernel.Bind<Core.Services.Administration.IUserService>().To<Core.Services.Administration.UserService>().InRequestScope();
            kernel.Bind<Core.Services.Administration.IRoleService>().To<Core.Services.Administration.RoleService>().InRequestScope();
            kernel.Bind<IMenuItemService>().To<MenuItemService>().InRequestScope();
            kernel.Bind<INewslineService>().To<NewslineService>().InRequestScope();

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["WingtipToys"].ConnectionString;
        }

        private static string ConnectionString { get { return System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseFile"].ConnectionString; } }

        private static IEnumerable<ActionRole> GetActionRoles()
        {
            var _assembly = Assembly.GetExecutingAssembly();

            IEnumerable<MethodInfo> methods = _assembly.GetTypes().
                            SelectMany(t => t.GetMethods())
                            .Where(m => m.GetCustomAttributes(typeof(RoleAttribute), true).Length > 0);

            var result = methods.Select(m => new ActionRole()
            {
                Action = m.Name.ToLower(),
                Role = ((RoleAttribute)m.GetCustomAttributes(typeof(RoleAttribute), true).FirstOrDefault()).Role,
                Controller = m.GetBaseDefinition().ReflectedType.Name.Replace("Controller", "").ToLower()
            }).ToList();

            return result;
        }
    }
}
