// This application entry point is based on ASP.NET Core new project templates and is included
// as a starting point for app host configuration.
// This file may need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core hosting, see https://docs.microsoft.com/aspnet/core/fundamentals/host/web-host

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using T034.Models;
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

            ActionRoles = GetActionRoles();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

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
