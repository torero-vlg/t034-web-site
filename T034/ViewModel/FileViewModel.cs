using System.ComponentModel;

namespace T034.ViewModel
{
    public class FileViewModel
    {
        public int Id { get; set; }

        [DisplayName("Документ")]
        public string Name { get; set; }
        
        [DisplayName("Ссылка")]
        public string Url {
            get { return string.Format("/Folder/Download?id={0}", Id); }
        }

        public string FileIcon
        {
            get
            {
                var extension = string.IsNullOrEmpty(Name) ? "" : Name.Substring(Name.LastIndexOf(".", System.StringComparison.Ordinal)).ToLower();
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
    }
}