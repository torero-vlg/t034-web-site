using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class MenuItemViewModel
    {
        [Display(Name = "Код")]
        public int Id { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        public string Active { get; set; }

        [Display(Name = "Порядковый номер")]
        public int OrderIndex { get; set; }
    }
}