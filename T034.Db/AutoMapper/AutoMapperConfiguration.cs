using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using AutoMapper;

namespace Db.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static List<T> StringToCollection<T>(string ids) where T : Entity.Entity, new()
        {
            return string.IsNullOrEmpty(ids) ? null : new List<T>(ids.Split(new string[] { "," }, StringSplitOptions.None).Select(n => new T { Id = Convert.ToInt32(n) }));
        }

        public static string IdsToString<T>(ICollection<T> collection) where T : Entity.Entity
        {
            return collection == null || !collection.Any() ? "" : collection.Select(n => n.Id.ToString()).Aggregate((i, j) => i.ToString() + "," + j.ToString());
        }

        public static void Configure(HttpServerUtility server)
        {
            foreach (var profile in Assembly.GetAssembly(typeof(AutoMapperConfiguration)).GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t)))
            {
                Mapper.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        }

        public static string GetSafeHtml(string htmlInputTxt)
        {
            var sb = new StringBuilder(HttpUtility.HtmlEncode(htmlInputTxt));

            sb.Replace("&lt;script&gt;", "");
            sb.Replace("&lt;/script&gt;", "");
            return sb.ToString();
        }
    }
}