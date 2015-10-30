using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using Db.Entity.Administration;
using T034.Tools.Attribute;
using T034.Tools.Auth;
using T034.ViewModel;

namespace T034.Controllers
{
    public class NewsController :BaseController
    {
        public ActionResult Index(int id)
        {
            var model = new NewsViewModel();

            var item = Db.Get<News>(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);

            return View(model);
        }

        [Role("Moderator")]
        public ActionResult List()
        {
            try
            {
                var items = Db.Select<News>();

                var model = new List<NewsViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                return View("ServerError", (object)"Получение списка");
            }
        }

        public ActionResult Newsline()
        {
            try
            {
                var items = Db.Select<News>();

                var model = new List<NewsViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                return View("ServerError", (object)"Получение списка");
            }
        }

        [HttpGet]
        [Role("Moderator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new NewsViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<News>(id.Value);
                model = Mapper.Map(item, model);
            }

            return View(model);
        }

        [Role("Moderator")]
        [ValidateInput(false)]
        public ActionResult AddOrEdit(NewsViewModel model)
        {
            var user = YandexAuth.GetUser(Request);

            //найдём пользователя в БД
            var userFromDb = Db.Where<User>(u => u.Email == user.default_email).FirstOrDefault();
            if (userFromDb != null)
            {
                var item = new News();
                if (model.Id > 0)
                {
                    item = Db.Get<News>(model.Id);
                }
                item = Mapper.Map(model, item);

                item.LogDate = DateTime.Now;
                item.User = new User { Id = userFromDb.Id };

                var result = Db.SaveOrUpdate(item);

                return RedirectToAction("List");
            }
            return View("ServerError", (object)"Не удалось определить пользователя");
        }

        [HttpGet]
        [Role("Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                var item = Db.Get<News>(id.Value);
                var result = Db.Delete(item);
            }
            return RedirectToAction("List");
        }
    }
}
