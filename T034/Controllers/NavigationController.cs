using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.Controllers
{
    public class NavigationController : BaseController
    {
        public ActionResult MainMenu()
        {
            try
            {
                var items = Db.Select<MenuItem>();

                var model = new List<MenuItemViewModel>();
                model = Mapper.Map(items, model);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView("ErrorMessage", (object)"Получение списка");
            }
        }

        public ActionResult ManagementMenu()
        {
            return PartialView();
        }
    }
}
