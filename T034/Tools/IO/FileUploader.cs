using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace T034.Tools.IO
{
    public class FileUploader : IFileUploader
    {
        private readonly string _storageRoot;

        public FileUploader(string storageRoot)
        {
            _storageRoot = storageRoot;
        }

        public ViewDataUploadFilesResult UploadPartialFile(HttpRequest request)
        {
            if (request.Form.Files.Count != 1) throw new Exception("Attempt to upload chunked file containing more than one fragment per request");

            var file = request.Form.Files[0];
            var status = SaveFile(file);
            return status;
        }

        public IEnumerable<ViewDataUploadFilesResult> UploadWholeFile(HttpRequest request)
        {
            var statuses = new List<ViewDataUploadFilesResult>();
            for (int i = 0; i < request.Form.Files.Count; i++)
            {
                var file = request.Form.Files[i];
                var status = SaveFile(file);
                statuses.Add(status);
            }

            return statuses;
        }

        private ViewDataUploadFilesResult SaveFile(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName.Replace("..", "."));
            var fullPath = Path.Combine(_storageRoot, fileName);

            if (!Directory.Exists(_storageRoot))
                Directory.CreateDirectory(_storageRoot);

            using (Stream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return new ViewDataUploadFilesResult()
            {
                name = file.FileName,
                size = file.Length,
                type = file.ContentType,
                url = "/Home/Download/" + file.FileName,
                delete_url = "/Home/Delete/" + file.FileName,
                delete_type = "GET",
            };
        }
    }
}
