namespace T034.Models
{
    public class FileUploadModel
    {
        /// <summary>
        /// Метод загрузки файлов
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Папка внутри папки для загрузки файлов
        /// </summary>
        public string Folder { get; set; }

        public override string ToString()
        {
            return Folder;
        }
    }
}