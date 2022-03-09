using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class StyleController : BaseController
    {
        public StyleController(IBaseDb db) 
            : base(db)
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
