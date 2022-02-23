using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Dto;
using T034.Core.Services.Administration;
using T034.Core.Services.Common;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;

        public UserController(AuthorizationRoot authorizationRoot,
            IRoleService roleService,
            IUserService userService,
            IBaseDb db)
            : base(authorizationRoot, db)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var list = _userService.Select();
                var model = new List<UserViewModel>();
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
            var model = new UserViewModel();
            if (id.HasValue)
            {
                var dto = _userService.Get(id.Value);
                model = Mapper.Map(dto, model);
            }
            //добавим те роли, которых нет у пользователя, но есть в БД
            foreach (var role in _roleService.Select())
            {
                if (model.UserRoles.Any(ur => ur.Code == role.Code))
                    continue;
                var roleViewModel = new RoleViewModel();
                roleViewModel = Mapper.Map(role, roleViewModel);
                roleViewModel.Selected = false;
                model.UserRoles.Add(roleViewModel);
            }
            
            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(UserViewModel model)
        {
            if (model.Id > 0)
            {
                _userService.Update(Mapper.Map<UserDto>(model));
            }
            else
            {
                _userService.Create(model.Name, model.Email, model.Password);
            }


            return RedirectToAction("List");
        }

        public ActionResult Index(int id)
        {
            var model = new UserViewModel();

            var item = _userService.Get(id);
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
                var result = _userService.Delete(id);
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
