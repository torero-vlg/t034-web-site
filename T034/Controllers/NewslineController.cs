using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using T034.Core.Dto;
using T034.Core.Services;
using T034.Core.Services.Common;
using T034.Tools.Attribute;
using T034.ViewModel;
using T034.Core.DataAccess;
using AutoMapper;

namespace T034.Controllers
{
    public class NewslineController : BaseController
    {
        private readonly INewslineService _newslineService;

        private readonly IMenuItemService _menuItemService;

        public NewslineController(IMenuItemService menuItemService,
            INewslineService newslineService,
            IBaseDb db, IMapper mapper)
            : base(db, mapper)
        {
            _menuItemService = menuItemService;
            _newslineService = newslineService;
        }

        [Role("Moderator")]
        public ActionResult List()
        {
            try
            {
                var list = _newslineService.Select();
                var model = new List<NewslineViewModel>();
                model = Mapper.Map(list, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        [HttpGet]
        [Role("Moderator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new NewslineViewModel();
            if (id.HasValue)
            {
                var dto = _newslineService.Get(id.Value);
                model = Mapper.Map(dto, model);
            }

            //TODO дублирует код из FolderController
            var menuItems = _menuItemService.Select();
            model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(menuItems);

            var byUrl = _menuItemService.ByUrl(model.IndexUrl);
            if (byUrl != null)
            {
                var selected = model.MenuItems.FirstOrDefault(m => m.Value == byUrl.Id.ToString());
                selected.Selected = true;
            }

            return View(model);
        }

        [Role("Moderator")]
        public ActionResult AddOrEdit(NewslineViewModel model)
        {
            if (model.Id > 0)
            {
                _newslineService.Update(Mapper.Map<NewslineDto>(model));
            }
            else
            {
                _newslineService.Create(Mapper.Map<NewslineDto>(model));
            }

            return RedirectToAction("List");
        }

        public ActionResult Index(int id)
        {
            try
            {
                var items = _newslineService.GetNews(id);
                var model = new List<NewsViewModel>();
                model = Mapper.Map<List<NewsViewModel>>(items);

                return View(model.OrderBy(m => m.LogDate));
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        [Role("Administrator")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _newslineService.Delete(id);
                if (result.Status != StatusOperation.Success)
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
