﻿using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Db.DataAccess;
using Db.Entity.Administration;
using Ninject;
using T034.Models;
using T034.Repository;

namespace T034.Tools.Auth
{
    public static class YandexAuth
    {
        public const string ClientId = "030edcedc0264dc188a18f4779642970";
        public const string Password = "8f71a3459a104af9a9e05e52af8b03cd";

        public const string InfoUrl = "https://login.yandex.ru/info";



        public static string GetAuthorizationCookie(HttpCookieCollection cookies, string code, IBaseDb db)
        {
            //var code = request.QueryString["code"];

            var stream = HttpTools.PostStream("https://oauth.yandex.ru/token",
                string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}", code, ClientId, Password));

            var model = SerializeTools.Deserialize<TokenModel>(stream);

            var userCookie = new HttpCookie("yandex_token")
            {
                Value = model.access_token,
                Expires = DateTime.Now.AddDays(30)
            };


            stream = HttpTools.PostStream(InfoUrl, string.Format("oauth_token={0}", userCookie.Value));
            var email = SerializeTools.Deserialize<UserModel>(stream).default_email;

            var user = db.SingleOrDefault<User>(u => u.Email == email);

            cookies.Set(userCookie);
            
            var rolesCookie = new HttpCookie("roles") {Value = string.Join(",", user.UserRoles.Select(r => r.Code)), Expires = DateTime.Now.AddDays(30)};
            cookies.Set(rolesCookie);
            
            return model.access_token;

        }

        public static UserModel GetUser(HttpRequestBase request)
        {
            var model = new UserModel{IsAutharization = false};
            try
            {
                var userCookie = request.Cookies["yandex_token"];
                if (userCookie != null)
                {
                    var stream = HttpTools.PostStream(InfoUrl, string.Format("oauth_token={0}", userCookie.Value));
                    model = SerializeTools.Deserialize<UserModel>(stream);
                    model.IsAutharization = true;
                }
            }
            catch (Exception)
            {
                //MonitorLog.WriteLog(ex.InnerException + ex.Message, MonitorLog.typelog.Error, true);
                model.IsAutharization = false;
            }

            return model;
        }
    }
}