using System.Collections.Generic;

namespace T034.ViewModel
{
    public class FolderViewModel
    {
        public string Name { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<FolderViewModel> SubDirectories { get; set; }
    }
}