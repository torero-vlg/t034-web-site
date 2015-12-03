using System.ComponentModel.DataAnnotations;

namespace T034.ViewModel
{
    public class PageViewModel
    {
        [Display(Name = "Код")]
        public int Id { get; set; }
        
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        
        [Display(Name = "Содержание")]
        public string Content { get; set; }
        
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
    }
}