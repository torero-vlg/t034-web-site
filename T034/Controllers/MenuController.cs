using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using T034.Core.Entity;
using T034.Core.Services;
using T034.Core.Services.Common;
using T034.Tools.Attribute;
using T034.ViewModel;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuController(IMenuItemService menuItemService,
            IBaseDb db) 
            : base(db)
        {
            _menuItemService = menuItemService;
        }

        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                //TODO MenuItemService
                var items = Db.Select<MenuItem>();

                var model = new List<MenuItemViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        [HttpGet]
        [Role("Administrator")]
        public ActionResult AddOrEdit(int? id)
        {
            //TODO MenuItemService
            var model = new MenuItemViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<MenuItem>(id.Value);
                model = Mapper.Map(item, model);
            }
            
            var items = Db.Select<MenuItem>();
            model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(items);

            model.MenuItems.Add(new SelectListItem { Value = null, Text = "Пункт главного меню", Selected = model.ParentId.HasValue == false});

            if (model.MenuItems.All(m => m.Selected == false))
            {
                var selected = model.MenuItems.FirstOrDefault(m => m.Value == model.ParentId.Value.ToString());
                selected.Selected = true;
            }

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(MenuItemViewModel model)
        {
            //TODO MenuItemService
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
            //TODO MenuItemService
            var model = new MenuItemViewModel();

            var item = Db.Get<MenuItem>(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult Set(MenuItemViewModel model)
        {
            MenuItem item = null;
            if (model.Id > 0)
            {
                item = Db.Get<MenuItem>(model.Id);
            }
            if (item == null)
            {
                Logger.Fatal($"Не найден указанный пункт меню: {model.Id}");
                return View("ServerError", (object)"Не найден указанный пункт меню");
            }
            item.Url = model.Url;

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        [Role("Administrator")]
        public ActionResult Delete(int menuItemId)
        {
            try
            {
                var result = _menuItemService.Delete(menuItemId);
                if (result.Status == StatusOperation.Error)
                {
                    Logger.Error(result.Message);
                    return View("ServerError", (object)result.Message);
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }
    }
}
