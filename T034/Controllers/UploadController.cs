using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Entity;
using T034.Core.Entity.Administration;
using T034.ViewModel;
using Microsoft.AspNetCore.Hosting;
using T034.Core.DataAccess;
using T034.Tools.IO;

namespace T034.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageUploader _imageUploader;

        public UploadController(IWebHostEnvironment webHostEnvironment, 
            IImageUploader imageUploader,
            IBaseDb db) 
            : base(db)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageUploader = imageUploader;
        }

        public void UploadNow()
        {
            _imageUploader.Upload(Request);
        }

        public ActionResult UploadPartial()
        {
            var images = _imageUploader.GetFiles().Select(image => new ImageViewModel { Alt = image.Alt, Url = Url.Content("/" + image.Path) });
            return View(images);
        }

        [HttpGet]
        public ActionResult Export()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Export(FileViewModel file)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var csvLines = System.IO.File.ReadAllLines(Path.Combine(contentRootPath, "/Upload/Temp/news.csv"));

            var siteUrl = "http://localhost:3893";

            var erorrs = "";


            for (int i = 0; i < csvLines.Length; i++)
            {
                var line = csvLines[i];
                var t = line.Split(new[] {"\";\""}, StringSplitOptions.None);

                if (t.Count() != 5)
                {
                    erorrs += i + ",";
                    continue;
                }
                var news = new News
                {
                    Title = t[0].Substring(1),
                    Resume = t[1]
                        .Replace("/sites/default/files/styles/large/public/images", siteUrl + "/Upload/Images")
                        .Replace("/sites/default/files", "/Upload/Files")
                        .Replace("http://box9-vlg.ru", siteUrl)
                        .Replace("\"\"", "\""),
                    Body = t[2]
                        .Replace("/sites/default/files/styles/large/public/images", siteUrl + "/Upload/Images")
                        .Replace("/sites/default/files", "/Upload/Files")
                        .Replace("http://box9-vlg.ru", siteUrl)
                        .Replace("\"\"", "\""),
                    LogDate = UnixTimeStampToDateTime(Convert.ToDouble(t[3])),
                    User = new User {Id = 2}
                };

                var result = Db.SaveOrUpdate(news);
            }
            return View();
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
