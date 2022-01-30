using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using T034.Core.Entity;
using T034.Core.Entity.Administration;
using T034.Core.Services;
using Ninject;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class NewsController : BaseController
    {
        [Inject]
        public INewslineService NewslineService { get; set; }

        public NewsController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

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
                var items = Db.Select<News>().OrderByDescending(n => n.LogDate);

                var model = new List<NewsViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        public ActionResult Newsline()
        {
            try
            {
                var items = Db.Select<News>().OrderByDescending(n => n.LogDate);

                var model = new List<NewsViewModel>();
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
        [Role("Moderator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new NewsViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<News>(id.Value);
                model = Mapper.Map(item, model);
            }

            //TODO дублирует код из FolderController
            var newslines = NewslineService.Select();
            model.Newslines = Mapper.Map<ICollection<SelectListItem>>(newslines);

            var newsline = NewslineService.Get(model.NewslineId);
            if (newsline != null)
            {
                var selected = model.Newslines.FirstOrDefault(m => m.Value == newsline.Id.ToString());
                selected.Selected = true;
            }

            return View(model);
        }

        [Role("Moderator")]
        [ValidateInput(false)]
        public ActionResult AddOrEdit(NewsViewModel model)
        {
            //найдём пользователя в БД
            var userFromDb = Db.Where<User>(u => u.Email == UserInfo.Email).FirstOrDefault();
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
