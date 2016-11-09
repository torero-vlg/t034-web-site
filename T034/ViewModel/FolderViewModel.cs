using System.Collections.Generic;
using System.Web.Mvc;

namespace T034.ViewModel
{
    public class FolderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<FolderViewModel> SubDirectories { get; set; }

        /// <summary>
        /// Назначеннй пункт меню
        /// </summary>
        public int? MenuItemId { get; set; }

        /// <summary>
        /// Список пунктов меню
        /// </summary>
        public ICollection<SelectListItem> MenuItems { get; set; }

        public string IndexUrl => $"/Folder/Index/{Id}";
        public string EditUrl => $"/Folder/Edit/{Id}";
    }
}