using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace T034.ViewModel
{
    /// <summary>
    /// Новостная лента
    /// </summary>
    public class NewslineViewModel
    {
        [Display(Name = "Идентификатор")]
        public int Id { get; set; }
        
        [Display(Name = "Название")]
        public string Name { get; set; }

        /// <summary>
        /// Список пунктов меню
        /// </summary>
        public ICollection<SelectListItem> MenuItems { get; set; }

        public string IndexUrl => $"/Newsline/Index/{Id}";
    }
}