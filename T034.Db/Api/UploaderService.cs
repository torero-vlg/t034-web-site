using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Db.Api.Common;
using Db.Api.Common.FileUpload;
using Db.Dto;
using Db.Entity;
using Db.Entity.Administration;
using NLog;

namespace Db.Api
{
    public interface IUploaderService
    {
        /// <summary>
        /// Загрузить файлы
        /// </summary>
        /// <returns></returns>
        List<ViewDataUploadFilesResult> Upload(string path, HttpRequestBase request, UserModel userModel);
    }

    public class UploaderService : AbstractService, IUploaderService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public List<ViewDataUploadFilesResult> Upload(string path, HttpRequestBase request, UserModel userModel)
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
                    var userFromDb = Db.SingleOrDefault<User>(u => u.Email == userModel.default_email);
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
    }
}
