using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;

namespace T034.Components.Navigations
{
    public class ManagementMenu : ViewComponent
    {
        private readonly IUserService _userService;

        public ManagementMenu(IUserService userService)
        {
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            var email = Request.Cookies["auth"];

            //если есть пользователь в БД, то показываем меню
            if (email != null)
            {
                var user = _userService.GetUser(email);
                if (user != null)
                {
                    //TODO use ViewModel in View - not Entity Model
                    return View(user);
                }
            }

            return Content(string.Empty);
        }
    }
}
