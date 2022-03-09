using System.Collections.Generic;
using T034.ViewModel;
using Microsoft.AspNetCore.Mvc;
using T034.Core.DataAccess;

namespace T034.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IBaseDb db) : base(db)
        {
        }

        /// <summary>
        /// Renders home page with login link.
        /// </summary>
        public ActionResult Logon(LogonViewModel model)
        {
           //TODO fill auth clients
            model.Clients = new List<LoginInfoModel>();
            return View(model);
        }

    }
}
