﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Db.Api;
using Db.Api.Common.Exceptions;
using Db.Entity;
using Ninject;
using OAuth2;
using OAuth2.Models;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class FolderController : BaseController
    {
        public FolderController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Inject]
        public IFileService FileService { get; set; }

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
            var path = Path.Combine(Server.MapPath($"~/{MvcApplication.FilesFolder}"));
            var r = FileService.Upload(path, Request, UserInfo.Email);
            //TODO надо что-то возвращать
            return Json(r);
        }

        [Role("Moderator")]
        public ActionResult DeleteFile(int id)
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
        public ActionResult DeleteFolder(FolderViewModel model)
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
        public ActionResult CreateFolder(FolderViewModel model)
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
