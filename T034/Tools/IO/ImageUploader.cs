using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace T034.Tools.IO
{
    /// <summary>
    /// Загрузчик картинок. Суть такая же как и для FileUploader. Различие в рабочих папках.
    /// Можно их объединить. Но надо решить разный биндинг при использовании, в одном случаем одна папка, в другом другая.
    /// </summary>
    public class ImageUploader : IImageUploader
    {
        private readonly string _storageRoot;
        private readonly string _imagesFolder;

        public ImageUploader(IWebHostEnvironment webHostEnvironment, string imagesFolder)
        {
            var contentRootPath = webHostEnvironment.ContentRootPath;
            _storageRoot = Path.Combine(contentRootPath, imagesFolder);
            _imagesFolder = imagesFolder;
        }

        public void Upload(HttpRequest request)
        {
            var file = request.Form.Files[0];
            var imageName = file.FileName;

            var path = Path.Combine(_storageRoot, imageName);

            if (!Directory.Exists(_storageRoot))
                Directory.CreateDirectory(_storageRoot);

            using Stream fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
        }

        public IEnumerable<ImageModel> GetFiles()
        {
            var images = Directory.GetFiles(_storageRoot).Select(x => new ImageModel
            {
                Path = $"{_imagesFolder}/{Path.GetFileName(x)}",
                Alt = Path.GetFileName(x)
            });

            return images;
        }
    }
}
