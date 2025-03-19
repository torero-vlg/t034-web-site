using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using T034.Tools.Attribute;
using T034.ViewModel;
using T034.Core.DataAccess;
using T034.Core.Services.Common;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace T034.Controllers
{
    public class BackupController : BaseController
    {
        private readonly IBackupService _backupService;

        public BackupController(
            IBaseDb db,
            IBackupService backupService, 
            IMapper mapper) 
            : base(db, mapper)
        {
            _backupService = backupService;
        }
        
        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var items = _backupService.GetBackups();

                var model = new List<BackupViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        [Role("Administrator")]
        public ActionResult Download(string name)
        {
            try
            {
                var file = _backupService.Download(name);

                Response.Headers.Append("Content-Disposition", "inline; filename=" + file.Name);

                return File(file.Bytes, GetMimeType(file.Name));
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Ошибка при загрузке файла");
            }
        }

        [Role("Administrator")]
        public ActionResult Delete(string name)
        {
            try
            {
                _backupService.Delete(name);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", "Ошибка при удалении файла.");
            }
        }

        [Role("Administrator")]
        public ActionResult Make()
        {
            return View();
        }

        [Role("Administrator")]
        public ActionResult Backup()
        {
            try
            {
                _backupService.MakeBackup();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return Json(new OperationResult { Status = StatusOperation.Error, Message = ex.Message });
            }
            return Json(new OperationResult { Status = StatusOperation.Success, Message = "Операция выполнена успешно" });
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
