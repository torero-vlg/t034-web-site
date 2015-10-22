using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity.Administration;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult List()
        {
            try
            {
                var items = Db.Select<User>();

                var model = new List<UserViewModel>();
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
            var model = new UserViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<User>(id.Value);
                model = Mapper.Map(item, model);
            }

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(UserViewModel model)
        {
            var item = new User();
            if (model.Id > 0)
            {
                item = Db.Get<User>(model.Id);
            }
            item = Mapper.Map(model, item);

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        public ActionResult Index(int id)
        {
            var model = new UserViewModel();

            var item = Db.Get<User>(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);

            return View(model);
        }
    }
}
