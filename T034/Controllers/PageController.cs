using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using T034.Core.Entity;
using T034.Core.Services;
using Ninject;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class PageController : BaseController
    {
        [Inject]
        public IMenuItemService MenuItemService { get; set; }

        public PageController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [HttpGet]
        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult AddOrEdit(int? id)
        {
            var model = new PageViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Page>(id.Value);
                model = Mapper.Map(item, model);

                //TODO дублирует код из FolderController
                var menuItems = MenuItemService.Select();
                model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(menuItems);

                var byUrl = MenuItemService.ByUrl(model.IndexUrl);
                if (byUrl != null)
                {
                    var selected = model.MenuItems.FirstOrDefault(m => m.Value == byUrl.Id.ToString());
                    selected.Selected = true;
                }
            }

            return View(model);
        }

        [ValidateInput(false)]
        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult AddOrEdit(PageViewModel model)
        {
            var item = new Page();


            item = Mapper.Map(model, item);

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult List()
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

        public Microsoft.AspNetCore.Mvc.ActionResult Index(int id)
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

        public Microsoft.AspNetCore.Mvc.ActionResult Preview(int id)
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
