using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        [Display(Name = "Родительский пункт")]
        public int? ParentId { get; set; }

        public ICollection<SelectListItem> MenuItems { get; set; }

        public ICollection<MenuItemViewModel> Childs { get; set; }
    }
}