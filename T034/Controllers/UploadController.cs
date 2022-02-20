using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Entity;
using T034.Core.Entity.Administration;
using OAuth2;
using T034.ViewModel;
using Microsoft.AspNetCore.Hosting;

namespace T034.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(AuthorizationRoot authorizationRoot, IWebHostEnvironment webHostEnvironment) : base(authorizationRoot)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void UploadNow()
        {
            var file = Request.Form.Files[0];
            var imageName = file.FileName;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var path = Path.Combine(webRootPath, "/Upload/Images", imageName);//TODO перенести путь в config

            using Stream fileStream = new FileStream(path, FileMode.Create);
            file.CopyTo(fileStream);

        }

        public ActionResult UploadPartial()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var appData = Path.Combine(webRootPath, "/Upload/Images");//TODO перенести путь в config
            var images = Directory.GetFiles(appData).Select(x => new ImageViewModel
            {
                Url = Url.Content("/Upload/Images/" + Path.GetFileName(x)),//TODO перенести путь в config
                Alt = Path.GetFileName(x)
            });
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

            var csvLines = System.IO.File.ReadAllLines(Path.Combine(webRootPath, "/Upload/Temp/news.csv"));

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
