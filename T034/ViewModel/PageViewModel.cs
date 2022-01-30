using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        /// <summary>
        /// Назначеннй пункт меню
        /// </summary>
        public int? MenuItemId { get; set; }

        /// <summary>
        /// Список пунктов меню
        /// </summary>
        public ICollection<SelectListItem> MenuItems { get; set; }

        public string IndexUrl => $"/Page/Index/{Id}";
    }
}