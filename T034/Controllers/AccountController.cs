using System.Collections.Generic;
using T034.ViewModel;
using Microsoft.AspNetCore.Mvc;
using T034.Core.DataAccess;
using AutoMapper;

namespace T034.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IBaseDb db, IMapper mapper) : base(db, mapper)
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
