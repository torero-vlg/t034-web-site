using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Db.Dto;
using Db.Services.Administration;
using Db.Services.Common;
using Ninject;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class RoleController : BaseController
    {
        [Inject]
        public IRoleService RoleService { get; set; }

        public RoleController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Tools.Attribute.Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var list = RoleService.Select();
                var model = new List<RoleViewModel>();
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
        [Tools.Attribute.Role("Administrator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new RoleViewModel();
            if (id.HasValue)
            {
                var dto = RoleService.Get(id.Value);
                model = Mapper.Map(dto, model);
            }

            return View(model);
        }

        [Tools.Attribute.Role("Administrator")]
        public ActionResult AddOrEdit(RoleViewModel model)
        {
            if (model.Id > 0)
            {
                RoleService.Update(Mapper.Map<RoleDto>(model));
            }
            else
            {
                RoleService.Create(Mapper.Map<RoleDto>(model));
            }


            return RedirectToAction("List");
        }

        [Tools.Attribute.Role("Administrator")]
        public ActionResult Index(int id)
        {
            var model = new RoleViewModel();

            var item = RoleService.Get(id);
            if (item == null)
            {
                return View("ServerError", (object)"Страница не найдена");
            }
            model = Mapper.Map(item, model);

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = RoleService.Delete(id);
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
                return View("ServerError", (object)"Ошибка");
            }
        }
    }
}
