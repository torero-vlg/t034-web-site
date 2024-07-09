using System;

namespace T034.ViewModel
{
    public class BackupViewModel
    {
        public string Name { get; set; }
        
        public string Url => $"/Backup/Download?name={Name}";

        public DateTime CreationTime { get; set; }

        public string Size { get; set; }

        public string FileIcon => "fa-file-archive-o";
    }
}