using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db.Entity;
using Db.Entity.Administration;
using T034.ViewModel;

namespace T034.Controllers
{
    public class UploadController : BaseController
    {
        public void UploadNow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                var imageName = upload.FileName;
                var path = Path.Combine(Server.MapPath("~/Upload/Images"), imageName);//TODO перенести путь в config
                upload.SaveAs(path);
            }
        }

        public ActionResult UploadPartial()
        {
            var appData = Server.MapPath("~/Upload/Images");//TODO перенести путь в config
            var images = Directory.GetFiles(appData).Select(x => new ImageViewModel
            {
                Url = Url.Content("/Upload/Images/" + Path.GetFileName(x))//TODO перенести путь в config
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
            var csvLines = System.IO.File.ReadAllLines(Server.MapPath("~/Upload/Temp/news.csv"));

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
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
