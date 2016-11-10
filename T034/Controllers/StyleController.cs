using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity.Administration;
using OAuth2;
using T034.ViewModel;

namespace T034.Controllers
{
    public class StyleController : BaseController
    {
        public StyleController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }
        
        public ActionResult LowVision(string styleName)
        {
            var userCookie = new HttpCookie("style")
            {
                Value = styleName,
                Expires = DateTime.Now.AddDays(30)
            };
            Response.Cookies.Set(userCookie);

            
            return RedirectToAction("Index", "Home");
        }
    }
}
