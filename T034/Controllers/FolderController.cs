using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using Db.Entity.Administration;
using T034.Tools.Attribute;
using T034.Tools.Auth;
using T034.Tools.FileUpload;
using T034.ViewModel;

namespace T034.Controllers
{
    public class FolderController : BaseController
    {
        public ActionResult Index(int? id)
        {
            //ViewData["MetMenuActive"] = folder.Contains("Методическая работа/") ? "active" : "";
            //ViewData["DocsMenuActive"] = folder.Contains("Документы/") ? "active" : "";


            var model = new FolderViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Folder>(id.Value);
                model = Mapper.Map(item, model);

                if (item != null)
                {
                    model = CreateFolderViewModel(model);
                    return View(model);
                }
                return View("ServerError", (object)string.Format("Отсутствует папка."));
            }

            model.SubDirectories = GetSubDirectories(null);

            return View(model);
        }

        [Role("Moderator")]
        public ActionResult Edit(int? id)
        {
            var model = new FolderViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Folder>(id.Value);
                model = Mapper.Map(item, model);
                
                if (item != null)
                {
                    model = CreateFolderViewModel(model);
                    return View(model);
                }
                return View("ServerError", (object)string.Format("Отсутствует папка."));
            }

            model.SubDirectories = GetSubDirectories(null);

            return View(model);
        }

        private FolderViewModel CreateFolderViewModel(FolderViewModel model)
        {
            var items = Mapper.Map<IEnumerable<FileViewModel>>(Db.Where<Files>(f => f.Folder.Id == model.Id));

            var subs = GetSubDirectories(model.Id);

            model.Files = items;
            model.SubDirectories = subs;
                
            return model;
        }

        private IEnumerable<FolderViewModel> GetSubDirectories(int? folderId)
        {
            var subDirectory = Db.Where<Folder>(f => f.ParentFolder.Id == folderId);

            var subs = subDirectory.Select(subDir => new FolderViewModel
                {
                    Id = subDir.Id,
                    Name = subDir.Name,
                    Files = Mapper.Map<IEnumerable<FileViewModel>>(Db.Where<Files>(ff => ff.Folder.Id == subDir.Id))
                }).ToList();
            return subs;
        }

        [HttpPost]
        [Role("Moderator")]
        public ActionResult UploadFile()
        {
            //TODO Api
            var path = Path.Combine(Server.MapPath(string.Format("~/{0}", MvcApplication.FilesFolder)));
            //path = Path.Combine(path, Request.Files.Keys[0]);
            var uploader = new Uploader(path);

            var r = new List<ViewDataUploadFilesResult>();
            if (Request.Files.Cast<string>().Any())
            {
                try
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

                    //запись в БД
                    var user = YandexAuth.GetUser(Request);

                    //найдём пользователя в БД
                    var userFromDb = Db.SingleOrDefault<User>(u => u.Email == user.default_email);
                    if (userFromDb != null)
                    {
                        foreach (var filesResult in statuses)
                        {
                            //TODO выделить в метод репозитория, запускать в одной транзакции
                            var fileByName = Db.SingleOrDefault<Files>(f => f.Name == filesResult.name);
                            if (fileByName != null)
                                Db.Delete(fileByName);

                            var item = new Files
                                {
                                    LogDate = DateTime.Now,
                                    Name = filesResult.name,
                                    Size = filesResult.size,
                                    User = new User {Id = userFromDb.Id},
                                    Folder = new Folder {Id = int.Parse(Request.Files.Keys[0])}
                                };

                            Db.SaveOrUpdate(item);    
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex);
                    throw;
                }
            }
            return Json(r);
        }

        [Role("Moderator")]
        public ActionResult DeleteFile(int id)
        {
            try
            {
                //TODO Api
                var item = Db.Get<Files>(id);
                var result = Db.Delete(item);

                var file = new FileInfo(Path.Combine(Server.MapPath(string.Format("~/{0}", MvcApplication.FilesFolder)), item.Name));
                file.Delete();
                
                return RedirectToAction("Edit", new { id = item.Folder.Id });
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)string.Format("Ошибка при удалении файла."));
            }
        }

        [Role("Moderator")]
        public ActionResult DeleteFolder(FolderViewModel model)
        {
            try
            {
                //TODO Api
                var item = Mapper.Map<Folder>(model);
                var result = Db.Delete(item);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)string.Format("Ошибка при удалении папки {0}.", model.Name));
            }

            return RedirectToAction("Edit", new { id = model.ParentFolderId });
        }

        [Role("Moderator")]
        public ActionResult CreateFolder(FolderViewModel model)
        {
            var user = YandexAuth.GetUser(Request);

            //найдём пользователя в БД
            var userFromDb = Db.SingleOrDefault<User>(u => u.Email == user.default_email);
            if (userFromDb != null)
            {
                var item = new Folder();
                if (model.Id > 0)
                {
                    item = Db.Get<Folder>(model.Id);
                }
                item = Mapper.Map(model, item);

                item.LogDate = DateTime.Now;
                item.User = new User { Id = userFromDb.Id };

                var result = Db.SaveOrUpdate(item);

                return RedirectToAction("Edit", new {id = result});
            }
            return View("ServerError", (object)"Не удалось определить пользователя");
        }

        public ActionResult Download(int id)
        {
            try
            {


                var item = Db.Get<Files>(id);
                if (item == null)
                {
                    //TODO обработка
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(string.Format("/{0}/{1}", MvcApplication.FilesFolder, item.Name)));
                string fileName = item.Name;

                item.DownloadCounter++;
                Db.SaveOrUpdate(item);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Ошибка при загрузке файла");
            }
        }
    }
}
