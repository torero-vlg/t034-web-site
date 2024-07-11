using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using T034.Core.Entity;
using T034.Core.Services;
using T034.Tools.Attribute;
using T034.ViewModel;
using T034.Core.DataAccess;
using AutoMapper;

namespace T034.Controllers
{
    public class PageController : BaseController
    {
        private readonly IMenuItemService _menuItemService;

        public PageController(IMenuItemService menuItemService,
            IBaseDb db, IMapper mapper)
            : base(db, mapper)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        [Role("Moderator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new PageViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Page>(id.Value);
                model = Mapper.Map(item, model);

                //TODO дублирует код из FolderController
                var menuItems = _menuItemService.Select();
                model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(menuItems);

                var byUrl = _menuItemService.ByUrl(model.IndexUrl);
                if (byUrl != null)
                {
                    var selected = model.MenuItems.FirstOrDefault(m => m.Value == byUrl.Id.ToString());
                    selected.Selected = true;
                }
            }

            return View(model);
        }

        [Role("Moderator")]
        public ActionResult AddOrEdit(PageViewModel model)
        {
            var item = new Page();


            item = Mapper.Map(model, item);

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        [Role("Moderator")]
        public ActionResult List()
        {
            try
            {
                var items = Db.Select<Page>();

                var model = new List<PageViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        public ActionResult Index(int id)
        {
            if (id > 99)
                return View("StaticPage" + id);
            var model = new PageViewModel();

            var item = Db.Get<Page>(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);
            
            return View(model);
        }

        public ActionResult Preview(int id)
        {
            var model = new PageViewModel();

            var item = Db.Get<Page>(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);

            return View(model);
        }

    }

   
}
