﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T034.Core.Api;
using T034.Core.Api.Common.Exceptions;
using T034.Core.Entity;
using T034.Core.Services;
using Ninject;
using OAuth2;
using OAuth2.Models;
using T034.Tools.Attribute;
using T034.Tools.IO;
using T034.ViewModel;

namespace T034.Controllers
{
    public class FolderController : BaseController
    {
        [Inject]
        public IMenuItemService MenuItemService { get; set; }

        public FolderController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Inject]
        public IFileService FileService { get; set; }

        public Microsoft.AspNetCore.Mvc.ActionResult Index(int? id)
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
        public Microsoft.AspNetCore.Mvc.ActionResult Edit(int? id)
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

            //TODO дублирует код из PageController
            var menuItems = MenuItemService.Select();
            model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(menuItems);
            
            var byUrl = MenuItemService.ByUrl(model.IndexUrl);
            if (byUrl != null)
            {
                var selected = model.MenuItems.FirstOrDefault(m => m.Value == byUrl.Id.ToString());
                selected.Selected = true;
            }

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
        public Microsoft.AspNetCore.Mvc.ActionResult UploadFile()
        {
            var result = Upload(Request);
            
            FileService.AddFile(result.Select(f => new T034.Core.Dto.FileDto { Name = f.name, Size = f.size }), UserInfo.Email, int.Parse(Request.Files.Keys[0]));
            //TODO надо что-то возвращать
            return Json(result);
        }

        private IEnumerable<ViewDataUploadFilesResult> Upload(HttpRequestBase request)
        {
            var path = Path.Combine(Server.MapPath($"~/{MvcApplication.FilesFolder}"));

            var statuses = new List<ViewDataUploadFilesResult>();
            var uploader = new FileUploader(path);
            if (request.Files.Cast<string>().Any())
            {
                try
                {
                    var headers = request.Headers;
                    if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    {
                        statuses.AddRange(uploader.UploadWholeFile(request));
                    }
                    else
                    {
                        statuses.Add(uploader.UploadPartialFile(request));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex);
                    throw;
                }
            }
            return statuses;
        }

        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult DeleteFile(int id)
        {
            try
            {
                var folder = FileService.DeleteFile(id, Server.MapPath(string.Format("~/{0}", MvcApplication.FilesFolder)));

                return RedirectToAction("Edit", new { id = folder.Id });
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)string.Format("Ошибка при удалении файла."));
            }
        }

        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult DeleteFolder(FolderViewModel model)
        {
            try
            {
                var item = Mapper.Map<Folder>(model);
                FileService.DeleteFolder(item);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object) $"Ошибка при удалении папки {model.Name}.");
            }

            return RedirectToAction("Edit", new { id = model.ParentFolderId });
        }

        [Role("Moderator")]
        public Microsoft.AspNetCore.Mvc.ActionResult CreateFolder(FolderViewModel model)
        {
            var item = new Folder();
            if (model.Id > 0)
            {
                item = FileService.GetFolder(model.Id); 
            }
            item = Mapper.Map(model, item);

            try
            {
                FileService.CreateFolder(UserInfo.Email, item);
            }
            catch (UserNotFoundException ex)
            {
                return View("ServerError", ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Произошла непредвиденная ошибка");
            }
            return RedirectToAction("Edit", new { id =  item.Id});
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Download(int id)
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
                Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
                return File(fileBytes, GetMimeType(fileName));
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Ошибка при загрузке файла");
            }
        }

        private string GetMimeType(string filename)
        {

            var extension = string.IsNullOrEmpty(filename) ? "" : filename.Substring(filename.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
            switch (extension)
            {
                case ".pdf": return "application/pdf";

                case ".jpeg": return "image/jpeg";
                case ".jpg": return "image/jpeg";
                case ".png": return "image/png";
                case ".tiff": return "image/tiff";

                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";

                case ".zip": return "application/zip";
                case ".rar": return "application/x-rar-compressed";
                case ".7z": return "application/zip";
                case ".txt": return "text/plain";
                case ".mp3": return "audio/mpeg";
                case ".avi": return "video/mpeg";
                case ".cs": return "text/plain";
                case ".ppt": return "application/vnd.ms-powerpoint";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
