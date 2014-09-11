using System;
using System.Web;
using Db.Tools;
using T034.Models;

namespace T034.Tools.Auth
{
    public static class YandexAuth
    {
        public const string ClientId = "030edcedc0264dc188a18f4779642970";
        public const string Password = "8f71a3459a104af9a9e05e52af8b03cd";

        public const string InfoUrl = "https://login.yandex.ru/info";

        public static TokenModel GetToken(HttpRequestBase request)
        {
            var code = request.QueryString["code"];

            var stream = HttpTools.PostStream("https://oauth.yandex.ru/token",
                string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}", code, ClientId, Password));

            var model = SerializeTools.Deserialize<TokenModel>(stream);

            return model;
        }

        public static HttpCookie TokenCookie(TokenModel model)
        {
            var userCookie = new HttpCookie("yandex_token")
            {
                Value = model.access_token,
                Expires = DateTime.Now.AddDays(30)
            };

            return userCookie;
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
            catch (Exception ex)
            {
                MonitorLog.WriteLog(ex.InnerException + ex.Message, MonitorLog.typelog.Error, true);
                model.IsAutharization = false;
            }

            return model;
        }
    }
}