using System.Collections.Generic;

namespace T034.ViewModel
{
    public class FolderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<FolderViewModel> SubDirectories { get; set; }
    }
}