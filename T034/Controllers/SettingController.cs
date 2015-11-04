using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class SettingController : BaseController
    {
        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var items = Db.Select<Setting>();

                var model = new List<SettingViewModel>();
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
            var model = new SettingViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Setting>(id.Value);
                model = Mapper.Map(item, model);
            }

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(SettingViewModel model)
        {
            var item = new Setting();
            if (model.Id > 0)
            {
                item = Db.Get<Setting>(model.Id);
            }
            item = Mapper.Map(model, item);

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        public ActionResult Index(int id)
        {
            //инициализация настроек
            if(Db.Where<Setting>(s => s.Code == "StartPage").FirstOrDefault() == null)
                Db.SaveOrUpdate(new Setting());
            
            return View();
        }
    }
}
