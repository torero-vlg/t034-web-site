using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Db.Api.Common;
using Db.Api.Common.Exceptions;
using Db.Api.Common.FileUpload;
using Db.Dto;
using Db.Entity;
using Db.Entity.Administration;
using NLog;

namespace Db.Api
{
    public interface IFileService
    {
        /// <summary>
        /// Загрузить файлы
        /// </summary>
        /// <returns></returns>
        List<ViewDataUploadFilesResult> Upload(string path, HttpRequestBase request, string email);

        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filesFolder">Папка с файлами</param>
        /// <returns>Папка, из которой удалили</returns>
        Folder DeleteFile(int id, string filesFolder);

        /// <summary>
        /// Удалить папку
        /// </summary>
        /// <param name="folder"></param>
        void DeleteFolder(Folder folder);

        /// <summary>
        /// Удалить папку
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="folder"></param>
        Folder CreateFolder(string email, Folder folder);

        /// <summary>
        /// Получить папку
        /// </summary>
        Folder GetFolder(int id);
    }

    public class FileService : AbstractService, IFileService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public List<ViewDataUploadFilesResult> Upload(string path, HttpRequestBase request, string email)
        {
            var r = new List<ViewDataUploadFilesResult>();
            var uploader = new Uploader(path);
            if (request.Files.Cast<string>().Any())
            {
                try
                {
                    var statuses = new List<ViewDataUploadFilesResult>();
                    var headers = request.Headers;
                    if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    {
                        uploader.UploadWholeFile(request, statuses);
                    }
                    else
                    {
                        uploader.UploadPartialFile(headers["X-File-Name"], request, statuses);
                    }
                    //JsonResult result = Json(statuses);
                    //result.ContentType = "text/plain";

                    //найдём пользователя в БД
                    var userFromDb = Db.SingleOrDefault<User>(u => u.Email == email);
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
                                User = new User { Id = userFromDb.Id },
                                Folder = new Folder { Id = int.Parse(request.Files.Keys[0]) }
                            };

                            Db.SaveOrUpdate(item);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex);
                    throw;
                }
            }
            return r;
        }

        public Folder DeleteFile(int id, string filesFolder)
        {
            var item = Db.Get<Files>(id);
            var result = Db.Delete(item);

            var file = new FileInfo(Path.Combine(filesFolder, item.Name));
            file.Delete();

            _logger.Info($"Удалён файл {file.FullName}");

            return item.Folder;
        }

        public void DeleteFolder(Folder folder)
        {
            var result = Db.Delete(folder);

            _logger.Info($"Удалена папка {folder}");
        }

        public Folder GetFolder(int id)
        {
            var item = Db.Get<Folder>(id);
            return item;
        }

        public Folder CreateFolder(string email, Folder folder)
        {
            //TODO Api
            //найдём пользователя в БД
            var userFromDb = Db.SingleOrDefault<User>(u => u.Email == email);
            if (userFromDb != null)
            {

                folder.LogDate = DateTime.Now;
                folder.User = new User { Id = userFromDb.Id };

                var result = Db.SaveOrUpdate(folder);

                _logger.Info($"Создана папка {folder}");
            }
            else
            {
                throw new UserNotFoundException(email);
            }
            return folder;
        }
    }
}
