using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuth2;

namespace T034.Controllers
{
    public class StyleController : BaseController
    {
        public StyleController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }
        
        public ActionResult Set(string styleName)
        {
            Response.Cookies.Append("style",
                styleName,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                });

            if(!string.IsNullOrEmpty(Request.Headers["Referer"].ToString()))
                return Redirect(Request.Headers["Referer"].ToString());
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
