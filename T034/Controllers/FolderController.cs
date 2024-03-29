﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using T034.Core.Api;
using T034.Core.Api.Common.Exceptions;
using T034.Core.Entity;
using T034.Core.Services;
using T034.Tools.Attribute;
using T034.Tools.IO;
using T034.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class FolderController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IMenuItemService _menuItemService;

        private readonly IFileService _fileService;

        private readonly IFileUploader _fileUploader;

        public FolderController(IWebHostEnvironment webHostEnvironment, 
            IMenuItemService menuItemService, 
            IFileService fileService, 
            IFileUploader fileUploader,
            IBaseDb db) 
            : base(db)
        {
            _webHostEnvironment = webHostEnvironment;
            _menuItemService = menuItemService;
            _fileService = fileService;
            _fileUploader = fileUploader;
        }

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

            //TODO дублирует код из PageController
            var menuItems = _menuItemService.Select();
            model.MenuItems = Mapper.Map<ICollection<SelectListItem>>(menuItems);
            
            var byUrl = _menuItemService.ByUrl(model.IndexUrl);
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
        public ActionResult UploadFile()
        {
            var result = Upload(Request);

            int.TryParse(Request.Form.Files[0].Name, out int folderId);
            
            _fileService.AddFile(result.Select(f => new Core.Dto.FileDto { Name = f.name, Size = f.size }), UserInfo.Email, folderId);
            //TODO надо что-то возвращать
            return Json(result);
        }

        private IEnumerable<ViewDataUploadFilesResult> Upload(HttpRequest request)
        {
            var statuses = new List<ViewDataUploadFilesResult>();
            if (request.Form.Files.Any())
            {
                try
                {
                    var headers = request.Headers;
                    if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    {
                        statuses.AddRange(_fileUploader.UploadWholeFile(request));
                    }
                    else
                    {
                        statuses.Add(_fileUploader.UploadPartialFile(request));
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
        public ActionResult DeleteFile(int id)
        {
            try
            {
                var folder = _fileService.DeleteFile(id);

                return RedirectToAction("Edit", new { id = folder.Id });
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
                var item = Mapper.Map<Folder>(model);
                _fileService.DeleteFolder(item);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object) $"Ошибка при удалении папки {model.Name}.");
            }

            return RedirectToAction("Edit", new { id = model.ParentFolderId });
        }

        [Role("Moderator")]
        public ActionResult CreateFolder(FolderViewModel model)
        {
            var item = new Folder();
            if (model.Id > 0)
            {
                item = _fileService.GetFolder(model.Id); 
            }
            item = Mapper.Map(model, item);

            try
            {
                _fileService.CreateFolder(UserInfo.Email, item);
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

        public ActionResult Download(int id)
        {
            try
            {
                var file = _fileService.Download(id);

                Response.Headers.Append("Content-Disposition", "inline; filename=" + file.Name);

                return File(file.Bytes, GetMimeType(file.Name));
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
