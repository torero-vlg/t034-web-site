using System.ComponentModel;

namespace T034.ViewModel
{
    public class FileViewModel
    {
        [DisplayName("Документ")]
        public string Name { get; set; }
        
        [DisplayName("Ссылка")]
        public string Url { get; set; }
    }
}