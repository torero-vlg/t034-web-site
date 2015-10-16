using System;

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
        public string Category { get; set; }

        /// <summary>
        /// Папка для загрузки файлов сущности с идентификатором Id
        /// </summary>
        public string Id { get; set; }

        
        /// <summary>
        /// Выделяет из входной строки название категории и ИД
        /// </summary>
        /// <param name="inputSring">Входная строка, например: Category-99</param>
        /// <returns></returns>
        public static FileUploadModel ParseParam(string inputSring)
        {
            var indexOfSeparator = 0;
            try
            {
                indexOfSeparator = inputSring.IndexOf("-");
            }
            catch
            {
                
            }
            
            var fileUploadModel = new FileUploadModel
                {
                    Category = inputSring.Substring(0, indexOfSeparator),
                    Id = inputSring.Substring(indexOfSeparator + 1)
                };
            return fileUploadModel;
        }

        public override string ToString()
        {
            return Category + "-" + Id;
        }
    }
}