using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using T034.Core.Entity;
using OAuth2;
using T034.ViewModel;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class NavigationController : BaseController
    {
        private readonly IUserService _userService;

        public NavigationController(AuthorizationRoot authorizationRoot, 
            IUserService userService, 
            IBaseDb db) 
            : base(authorizationRoot, db)
        {
        }

        public ActionResult MainMenu()
        {
            try
            {
                var items = Db.Where<MenuItem>(m => m.Parent == null);

                var model = new List<MenuItemViewModel>();
                model = Mapper.Map(items, model);

                foreach (var menuItem in model)
                {
                    var subs = Db.Where<MenuItem>(m => m.Parent.Id == menuItem.Id);
                    menuItem.Childs = Mapper.Map<ICollection<MenuItemViewModel>>(subs).OrderBy(m => m.OrderIndex);
                }

                return PartialView(model.OrderBy(m => m.OrderIndex));
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return PartialView("ErrorMessage", (object)"Получение списка");
            }
        }

        public ActionResult ManagementMenu()
        {
            //если есть пользователь в БД, то показываем меню
            if (UserInfo != null)
            {
                Logger.Trace(UserInfo.Email);
                var user = _userService.GetUser(UserInfo.Email);
                if (user != null)
                {
                    Logger.Trace(user.Email);
                    return PartialView(user);
                }
            }
            return null;
        }
    }
}
