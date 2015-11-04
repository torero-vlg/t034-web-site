using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Db.Tools;
using T034.Tools.Attribute;
using T034.Tools.FileUpload;
using T034.ViewModel;

namespace T034.Controllers
{
    public class FolderController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder">Папка, например images/</param>
        /// <returns></returns>
        public ActionResult Index(string folder = "")
        {
            ViewData["MetMenuActive"] = folder.Contains("Методическая работа/") ? "active" : "";
            ViewData["DocsMenuActive"] = folder.Contains("Документы/") ? "active" : "";


            var directory = new DirectoryInfo(Server.MapPath(string.Format("/{0}/{1}", MvcApplication.FilesFolder, folder)));
            if (directory.Exists)
            {
                var model = CreateFolderViewModel(folder, directory);
                return View(model);
            }
            return View("ServerError", (object)string.Format("Отсутствует папка {0}.", directory.Name));
        }

        [Role("Moderator")]
        public ActionResult Edit(string folder = "")
        {
            var directory = new DirectoryInfo(Server.MapPath(string.Format("/{0}/{1}", MvcApplication.FilesFolder, folder)));
            if (directory.Exists)
            {
                var model = CreateFolderViewModel(folder, directory);
                return View(model);
            }
            return View("ServerError", (object)string.Format("Отсутствует папка {0}.", directory.Name));
        }

        private static FolderViewModel CreateFolderViewModel(string folder, DirectoryInfo directory)
        {
            var files = directory.GetFiles().Select(f => f.Name);
            var items =
                files.Select(
                    file =>
                    new FileViewModel
                        {
                            Url = string.Format("/{0}/{1}", MvcApplication.FilesFolder, folder) + file,
                            Name = file
                        });

            var subDirectory = directory.GetDirectories();

            var subs = subDirectory.Select(subDir => new FolderViewModel
                {
                    Name = subDir.Name,
                    Files = subDir.GetFiles().Select(f => f.Name).Select(file => new FileViewModel
                        {
                            Url = string.Format("/{0}/{1}/", MvcApplication.FilesFolder, subDir.Name) + file, Name = file
                        })
                }).ToList();

            var model = new FolderViewModel
                {
                    Name = folder,
                    Files = items,
                    SubDirectories = subs
                };
            return model;
        }

        [HttpPost]
        [Role("Moderator")]
        public ActionResult UploadFile()
        {
            var path = Path.Combine(Server.MapPath(string.Format("~/{0}", MvcApplication.FilesFolder)));
            path = Path.Combine(path, Request.Files.Keys[0]);
            var uploader = new Uploader(path);

            var r = new List<ViewDataUploadFilesResult>();
            if (Request.Files.Cast<string>().Any())
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;
                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    uploader.UploadWholeFile(Request, statuses);
                }
                else
                {
                    uploader.UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }
                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";
                return result;
            }
            return Json(r);
        }

        [Role("Moderator")]
        public ActionResult DeleteFile(string url)
        {
            string folder;

            try
            {
                var file = new FileInfo(Server.MapPath(url));
                folder = file.Directory.Name;
                file.Delete();
            }
            catch (Exception ex)
            {
                MonitorLog.WriteLog(ex.Message, MonitorLog.typelog.Error, true);
                return View("ServerError", (object) string.Format("Ошибка при удалении файла {0}.", url));
            }

            return RedirectToAction("Edit", new { folder = folder + "/" });
        }
    }
}
