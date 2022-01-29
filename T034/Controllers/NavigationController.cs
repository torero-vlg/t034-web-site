using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Db.Api;
using Db.Entity;
using Ninject;
using OAuth2;
using T034.ViewModel;

namespace T034.Controllers
{
    public class NavigationController : BaseController
    {
        public NavigationController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Inject]
        public IUserService UserService { get; set; }

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
                var user = UserService.GetUser(UserInfo.Email);
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
