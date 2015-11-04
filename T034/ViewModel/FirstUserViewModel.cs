using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class FirstUserViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "YandexClientId")]
        public string YandexClientId { get; set; }

        [Display(Name = "YandexPassword")]
        public string YandexPassword { get; set; }
    }
}