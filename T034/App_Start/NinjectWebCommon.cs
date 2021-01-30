using System.Configuration;
using Db;
using Db.Api;
using Db.DataAccess;
using Db.Services;
using OAuth2;
using T034.Repository;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(T034.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(T034.App_Start.NinjectWebCommon), "Stop")]

namespace T034.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
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
            kernel.Bind<AuthorizationRoot>().To<AuthorizationRoot>().InRequestScope();

            kernel.Bind<Db.Services.Administration.IUserService>().To<Db.Services.Administration.UserService>().InRequestScope();
            kernel.Bind<Db.Services.Administration.IRoleService>().To<Db.Services.Administration.RoleService>().InRequestScope();
            kernel.Bind<IMenuItemService>().To<MenuItemService>().InRequestScope();
            kernel.Bind<INewslineService>().To<NewslineService>().InRequestScope();

        }

        private static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["DatabaseFile"].ConnectionString; } }
    }
}
