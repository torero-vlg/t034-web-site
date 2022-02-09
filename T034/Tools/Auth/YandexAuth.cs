﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using T034.Core.DataAccess;
using T034.Core.Dto;
using T034.Core.Entity;
using T034.Core.Entity.Administration;
using T034.Models;

namespace T034.Tools.Auth
{
    public static class YandexAuth
    {
        //public const string ClientId = "030edcedc0264dc188a18f4779642970";
        //public const string Password = "8f71a3459a104af9a9e05e52af8b03cd";
        //public const string ClientId = "6db29766a62c4da68f28aae2ccf4e091";
        //public const string Password = "4b8419836fc54277b2e8b3b954e6de44";
        public const string InfoUrl = "https://login.yandex.ru/info";



        public static string GetAuthorizationCookie(IResponseCookies cookies, string code, IBaseDb db)
        {
            //var code = request.QueryString["code"];

            var clientId = db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId").Value;
            var password = db.SingleOrDefault<Setting>(s => s.Code == "YandexPassword").Value;

            var stream = HttpTools.PostStream("https://oauth.yandex.ru/token",
                string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}", code, clientId, password));

            var model = SerializeTools.Deserialize<TokenModel>(stream);

            stream = HttpTools.PostStream(InfoUrl, string.Format("oauth_token={0}", model.access_token));
            var email = SerializeTools.Deserialize<UserModel>(stream).default_email;

            var user = db.SingleOrDefault<User>(u => u.Email == email);

            cookies.Append("yandex_token",
               model.access_token,
               new CookieOptions
               {
                   Expires = DateTime.Now.AddDays(30)
               });

            cookies.Append("roles",
               string.Join(",", user.UserRoles.Select(r => r.Code)),
               new CookieOptions
               {
                   Expires = DateTime.Now.AddDays(30)
               });

            return model.access_token;

        }

        //public static UserModel GetUser(HttpRequestBase request)
        //{
        //    var model = new UserModel{IsAutharization = false};
        //    try
        //    {
        //        var userCookie = request.Cookies["yandex_token"];
        //        if (userCookie != null)
        //        {
        //            var stream = HttpTools.PostStream(InfoUrl, string.Format("oauth_token={0}", userCookie.Value));
        //            model = SerializeTools.Deserialize<UserModel>(stream);
        //            model.IsAutharization = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //MonitorLog.WriteLog(ex.InnerException + ex.Message, MonitorLog.typelog.Error, true);
        //        model.IsAutharization = false;
        //    }

        //    return model;
        //}
    }
}