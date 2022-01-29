using System;
using System.ComponentModel;

namespace T034.ViewModel
{
    public class FileViewModel
    {
        public int Id { get; set; }

        [DisplayName("Документ")]
        public string Name { get; set; }
        
        [DisplayName("Ссылка")]
        public string Url => $"/Folder/Download?id={Id}";

        [DisplayName("Пользователь")]
        public string User { get; set; }

        [DisplayName("Пользователь")]
        public int UserId { get; set; }

        [DisplayName("Дата")]
        public DateTime LogDate { get; set; }

        [DisplayName("Количество скачиваний")]
        public int DownloadCounter { get; set; }

        [DisplayName("Размер")]
        public string Size { get; set; }

        public string FileIcon
        {
            get
            {
                var extension = string.IsNullOrEmpty(Name) ? "" : Name.Substring(Name.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
                switch (extension)
                {
                    case ".pdf": return "fa-file-pdf-o";
                    case ".jpeg": return "fa-file-image-o";
                    case ".jpg": return "fa-file-image-o";
                    case ".png": return "fa-file-image-o";
                    case ".tiff": return "fa-file-image-o";
                    case ".doc": return "fa-file-word-o";
                    case ".docx": return "fa-file-word-o";
                    case ".xls": return "fa-file-excel-o";
                    case ".xlsx": return "fa-file-excel-o";
                    case ".zip": return "fa-file-archive-o";
                    case ".rar": return "fa-file-archive-o";
                    case ".7z": return "fa-file-archive-o";
                    case ".txt": return "fa-file-text-o";
                    case ".mp3": return "fa-file-audio-o";
                    case ".avi": return "fa-file-video-o";
                    case ".cs": return "fa-file-code-o";
                    case ".ppt": return "fa-file-powerpoint-o";
                    default:
                        return "fa-file-o";
                }
            }
        }
    }
}