﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AutoMapper;
using Db.Entity;

namespace T034.AutoMapper
{
    public static class AutoMapperWebConfiguration
    {
        public static List<T> StringToCollection<T>(string ids) where T : Entity, new()
        {
            return string.IsNullOrEmpty(ids) ? null : new List<T>(ids.Split(new string[] { "," }, StringSplitOptions.None).Select(n => new T { Id = Convert.ToInt32(n) }));
        }

        public static string IdsToString<T>(ICollection<T> collection) where T : Entity
        {
            return collection == null || !collection.Any() ? "" : collection.Select(n => n.Id.ToString()).Aggregate((i, j) => i.ToString() + "," + j.ToString());
        }

        public static void Configure(HttpServerUtility server)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new RoleProfile());
                cfg.AddProfile(new NewsProfile());
                cfg.AddProfile(new PageProfile());
                cfg.AddProfile(new MenuItemProfile());
                cfg.AddProfile(new SettingProfile());
                cfg.AddProfile(new FilesProfile());
            });
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