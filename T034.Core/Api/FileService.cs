using System;
using System.Collections.Generic;
using System.IO;
using T034.Core.Api.Common;
using T034.Core.Api.Common.Exceptions;
using T034.Core.Dto;
using T034.Core.Entity;
using T034.Core.Entity.Administration;
using NLog;
using T034.Core.DataAccess;

namespace T034.Core.Api
{
    public interface IFileService
    {
        /// <summary>
        /// Добавить записи о файлах в БД
        /// </summary>
        /// <returns></returns>
        void AddFile(IEnumerable<FileDto> files, string email, int? folderId = null);

        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Папка, из которой удалили</returns>
        Folder DeleteFile(int id);

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

        /// <summary>
        /// Скачать файл
        /// </summary>
        DownloadFileDto Download(int id);
    }

    public class FileService : AbstractService, IFileService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _storageRoot;

        public FileService(IBaseDb db, string storageRoot)
            : base(db)
        {
            _storageRoot = storageRoot;
        }


        public void AddFile(IEnumerable<FileDto> files, string email, int? folderId = null)
        {
            try
            {
                //найдём пользователя в БД
                var userFromDb = Db.SingleOrDefault<User>(u => u.Email == email);
                if (userFromDb != null)
                {
                    foreach (var file in files)
                    {
                        //TODO выделить в метод репозитория, запускать в одной транзакции
                        var fileByName = Db.SingleOrDefault<Files>(f => f.Name == file.Name);
                        if (fileByName != null)
                            Db.Delete(fileByName);

                        var item = new Files
                        {
                            LogDate = DateTime.Now,
                            Name = file.Name,
                            Size = file.Size,
                            User = new User { Id = userFromDb.Id },
                            Folder = folderId.HasValue ? new Folder { Id = folderId.Value } : null
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

        public Folder DeleteFile(int id)
        {
            var item = Db.Get<Files>(id);
            var result = Db.Delete(item);

            var file = new FileInfo(Path.Combine(_storageRoot, item.Name));
            file.Delete();

            _logger.Info($"Удалён файл {file.FullName}");

            return item.Folder;
        }

        public void DeleteFolder(Folder folder)
        {
            var childFolders = Db.Where<Folder>(f => f.ParentFolder.Id == folder.Id);
            foreach (var child in childFolders)
                DeleteFolder(child);

            var files = Db.Where<Files>(f => f.Folder.Id == folder.Id);
            foreach (var file in files)
                DeleteFile(file.Id);

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

        public DownloadFileDto Download(int id)
        {
            var item = Db.Get<Files>(id);
            if (item == null)
            {
                throw new Exception("Файл не найден");
            }

            var path = Path.Combine(_storageRoot, item.Name);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = item.Name;

            item.DownloadCounter++;
            Db.SaveOrUpdate(item);

            return new DownloadFileDto { Bytes = fileBytes, Name = fileName };
        }
    }
}
