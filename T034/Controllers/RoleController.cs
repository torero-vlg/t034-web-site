using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Dto;
using T034.Core.Services.Administration;
using T034.Core.Services.Common;
using T034.Tools.Attribute;
using T034.ViewModel;
using T034.Core.DataAccess;
using AutoMapper;

namespace T034.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService,
            IBaseDb db, IMapper mapper) 
            : base(db, mapper)
        {
            _roleService = roleService;
        }

        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var list = _roleService.Select();
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
        [Role("Administrator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new RoleViewModel();
            if (id.HasValue)
            {
                var dto = _roleService.Get(id.Value);
                model = Mapper.Map(dto, model);
            }

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(RoleViewModel model)
        {
            if (model.Id > 0)
            {
                _roleService.Update(Mapper.Map<RoleDto>(model));
            }
            else
            {
                _roleService.Create(Mapper.Map<RoleDto>(model));
            }


            return RedirectToAction("List");
        }

        [Role("Administrator")]
        public ActionResult Index(int id)
        {
            var model = new RoleViewModel();

            var item = _roleService.Get(id);
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
                var result = _roleService.Delete(id);
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
