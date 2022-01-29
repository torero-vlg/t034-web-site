using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Db.Dto;
using Db.Services.Administration;
using Db.Services.Common;
using Ninject;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class UserController : BaseController
    {
        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IRoleService RoleService { get; set; }

        public UserController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var list = UserService.Select();
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
                var dto = UserService.Get(id.Value);
                model = Mapper.Map(dto, model);
            }
            //добавим те роли, которых нет у пользователя, но есть в БД
            foreach (var role in RoleService.Select())
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
                UserService.Update(Mapper.Map<UserDto>(model));
            }
            else
            {
                UserService.Create(model.Name, model.Email, model.Password);
            }


            return RedirectToAction("List");
        }

        public ActionResult Index(int id)
        {
            var model = new UserViewModel();

            var item = UserService.Get(id);
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
                var result = UserService.Delete(id);
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
