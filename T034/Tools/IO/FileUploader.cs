using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace T034.Tools.IO
{
    public class FileUploader
    {
        private readonly string _storageRoot;

        public FileUploader(string storageRoot)
        {
            _storageRoot = storageRoot;
        }

        public ViewDataUploadFilesResult UploadPartialFile(HttpRequest request)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

            HttpPostedFileBase file = request.Files[0];
            var status = SaveFile(file);
            return status;
        }

        public IEnumerable<ViewDataUploadFilesResult> UploadWholeFile(HttpRequest request)
        {
            var statuses = new List<ViewDataUploadFilesResult>();
            for (int i = 0; i < request.Files.Count; i++)
            {
                HttpPostedFileBase file = request.Files[i];
                var status = SaveFile(file);
                statuses.Add(status);
            }

            return statuses;
        }

        private ViewDataUploadFilesResult SaveFile(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName.Replace("..", "."));
            var fullPath = Path.Combine(_storageRoot, fileName);

            if (!Directory.Exists(_storageRoot))
                Directory.CreateDirectory(_storageRoot);

            file.SaveAs(fullPath);

            return new ViewDataUploadFilesResult()
            {
                name = file.FileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/Home/Download/" + file.FileName,
                delete_url = "/Home/Delete/" + file.FileName,
                delete_type = "GET",
            };
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }
}
