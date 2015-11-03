using System.ComponentModel;

namespace T034.ViewModel
{
    public class FileViewModel
    {

        public string FileIcon 
        {
            get
            {
                var extension = Url.Substring(Url.LastIndexOf(".")).ToLower();
                switch (extension)
                {
                    case ".pdf": return Resource.IconPdf;
                    case ".jpeg": return Resource.IconImage;
                    case ".doc": return Resource.IconOfficeDocument;
                    case ".docx": return Resource.IconOfficeDocument;
                    default:
                        return Resource.IconFile;
                }
            }
        }

        [DisplayName("Документ")]
        public string Name { get; set; }
        
        [DisplayName("Ссылка")]
        public string Url { get; set; }
    }
}