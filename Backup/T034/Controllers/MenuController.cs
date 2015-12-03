using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class MenuController : BaseController
    {
        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var items = Db.Select<MenuItem>();

                var model = new List<MenuItemViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                return View("ServerError", (object)"Получение списка");
            }
        }

        [HttpGet]
        [Role("Administrator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new MenuItemViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<MenuItem>(id.Value);
                model = Mapper.Map(item, model);
            }
            
            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(MenuItemViewModel model)
        {
            var item = new MenuItem();
            if (model.Id > 0)
            {
                item = Db.Get<MenuItem>(model.Id);
            }
            item = Mapper.Map(model, item);

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        public ActionResult Index(int id)
        {
            var model = new MenuItemViewModel();

            var item = Db.Get<MenuItem>(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);

            return View(model);
        }
    }
}
