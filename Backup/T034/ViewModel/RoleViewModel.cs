using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class RoleViewModel
    {
        [Display(Name = "Идентификатор")]
        public int Id { get; set; }
        
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Display(Name = "Код")]
        public string Code { get; set; }
        
        public bool Selected { get; set; }
    }
}