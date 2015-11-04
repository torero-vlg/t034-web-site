using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class SettingViewModel
    {
        [Display(Name = "Идентификатор")]
        public int Id { get; set; }
        
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Display(Name = "Код")]
        public string Code { get; set; }

        [Display(Name = "Значение")]
        public string Value { get; set; }
    }
}