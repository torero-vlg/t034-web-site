﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using T034.Core.Dto;
using T034.Core.Services;
using T034.Core.Services.Common;
using Ninject;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class NewslineController : BaseController
    {
        [Inject]
        public INewslineService NewslineService { get; set; }

        [Inject]
        public IMenuItemService MenuItemService { get; set; }

        public NewslineController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult List()
        {
            try
            {
                var list = NewslineService.Select();
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
        public Microsoft.AspNetCore.Mvc.ActionResult AddOrEdit(int? id)
        {
            var model = new NewslineViewModel();
            if (id.HasValue)
            {
                var dto = NewslineService.Get(id.Value);
                model = Mapper.Map(dto, model);
            }

            //TODO дублирует код из FolderController
            var menuItems = MenuItemService.Select();
            model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(menuItems);

            var byUrl = MenuItemService.ByUrl(model.IndexUrl);
            if (byUrl != null)
            {
                var selected = model.MenuItems.FirstOrDefault(m => m.Value == byUrl.Id.ToString());
                selected.Selected = true;
            }

            return View(model);
        }

        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult AddOrEdit(NewslineViewModel model)
        {
            if (model.Id > 0)
            {
                NewslineService.Update(Mapper.Map<NewslineDto>(model));
            }
            else
            {
                NewslineService.Create(Mapper.Map<NewslineDto>(model));
            }

            return RedirectToAction("List");
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Index(int id)
        {
            try
            {
                var items = NewslineService.GetNews(id);
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
        public Microsoft.AspNetCore.Mvc.ActionResult Delete(int id)
        {
            try
            {
                var result = NewslineService.Delete(id);
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
