﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class PageController : BaseController
    {
        [HttpGet]
        [Role("Moderator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new PageViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Page>(id.Value);
                model = Mapper.Map(item, model);
            }

            return View(model);
        }

        [ValidateInput(false)]
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
