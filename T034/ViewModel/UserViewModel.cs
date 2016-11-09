using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            UserRoles = new List<RoleViewModel>();
        }
        

        [Display(Name = "Код")]
        public int Id { get; set; }
        
        [Display(Name = "Имя")]
        public string Name { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public List<RoleViewModel> UserRoles { get; set; }
    }
}