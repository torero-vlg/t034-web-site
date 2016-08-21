using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class LogonViewModel
    {
        [Display(Name = "Имя пользователя")]
        public int Login { get; set; }
        
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public IEnumerable<LoginInfoModel> Clients { get; set; } 
    }
}