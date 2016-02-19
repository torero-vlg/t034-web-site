using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using Db.Entity.Administration;
using T034.Tools.Auth;
using T034.ViewModel;

namespace T034.Controllers
{
    public class NavigationController : BaseController
    {
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
                    menuItem.Childs = Mapper.Map<ICollection<MenuItemViewModel>>(subs);
                }

                return PartialView(model.OrderBy(m => m.OrderIndex));
            }
            catch (Exception ex)
            {
                return PartialView("ErrorMessage", (object)"Получение списка");
            }
        }

        public ActionResult ManagementMenu()
        {
            //если есть пользователь в БД, то показываем меню
            var user = YandexAuth.GetUser(Request);

            //найдём пользователя в БД
            var userFromDb = Db.Where<User>(u => u.Email == user.default_email).FirstOrDefault();
            if (userFromDb != null)
            {
                return PartialView();
            }

            return null;
        }
    }
}
