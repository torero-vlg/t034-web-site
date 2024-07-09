using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NLog;
using T034.Core.Dto;
using T034.Core.Entity;

namespace T034.Core.Api
{
    public interface IBackupService
    {
        /// <summary>
        /// Сделать бекап
        /// </summary>
        /// <returns></returns>
        void MakeBackup();
        
        /// <summary>
        /// Список бекапов
        /// </summary>
        /// <returns></returns>
        IEnumerable<BackupDto> GetBackups();

        /// <summary>
        /// Скачать бекап
        /// </summary>
        /// <returns></returns>
        DownloadFileDto Download(string name);

        /// <summary>
        /// Удалить бекап
        /// </summary>
        /// <returns></returns>
        void Delete(string name);
    }

    public class BackupService : IBackupService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _contentRootPath;

        public BackupService(string contentRootPath)
        {
            _contentRootPath = contentRootPath;
        }

        /// <summary>
        /// Создать бекап из текущей папки
        /// </summary>
        public void MakeBackup()
        {
            try
            {
                _logger.Trace($"Backup folder: {_contentRootPath}");

                var backupDirectory = Path.Combine(_contentRootPath, "_backups");
                if (!Directory.Exists(backupDirectory))
                    Directory.CreateDirectory(backupDirectory);

                var backupFileName = Path.Combine(backupDirectory, $"backup_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.zip");
                var backupFilePath = Path.Combine(backupDirectory, backupFileName);

                using (var zipToOpen = new FileStream(backupFilePath, FileMode.Create))
                {
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                    {
                        AddDirectoryToArchive(archive, _contentRootPath, _contentRootPath, "_backups");
                    }
                }

                _logger.Trace($"Backup created at: {backupFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
                throw;
            }
        }

        /// <summary>
        /// Добавляет содержимое папки в архив, исключая указанную папку
        /// </summary>
        /// <param name="archive">Архив</param>
        /// <param name="sourceDir">Папка для архивирования</param>
        /// <param name="baseDir">Базовая папка</param>
        /// <param name="excludeDir">Исключаемая папка</param>
        private static void AddDirectoryToArchive(ZipArchive archive, string sourceDir, string baseDir, string excludeDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string entryName = Path.GetRelativePath(baseDir, file.FullName);
                archive.CreateEntryFromFile(file.FullName, entryName);
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (subDir.Name.Equals(excludeDir, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                AddDirectoryToArchive(archive, subDir.FullName, baseDir, excludeDir);
            }
        }

        /// <summary>
        /// Создать бекап из текущей папки
        /// </summary>
        public IEnumerable<BackupDto> GetBackups()
        {
            try
            {
                var backupDirectory = Path.Combine(_contentRootPath, "_backups");
                if (!Directory.Exists(backupDirectory))
                {
                    _logger.Trace($"Directory '{backupDirectory}' not exists");
                    return Enumerable.Empty<BackupDto>();

                }

                var dir = new DirectoryInfo(backupDirectory);
                return dir.GetFiles().Select(f => new BackupDto { Name = f.Name, Size = f.Length, CreationTime = f.CreationTime });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
                throw;
            }
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DownloadFileDto Download(string name)
        {
            var backupDirectory = Path.Combine(_contentRootPath, "_backups");
            var backupFilePath = Path.Combine(backupDirectory, name);

            if (!Directory.Exists(backupDirectory) || !File.Exists(backupFilePath))
                throw new Exception($"Файл '{name}' не найден");

            byte[] fileBytes = File.ReadAllBytes(backupFilePath);

            return new DownloadFileDto { Bytes = fileBytes, Name = name };
        }

        public void Delete(string name)
        {
            var backupDirectory = Path.Combine(_contentRootPath, "_backups");
            var backupFilePath = Path.Combine(backupDirectory, name);

            if (!Directory.Exists(backupDirectory) || !File.Exists(backupFilePath))
                throw new Exception($"Файл '{name}' не найден");

            var file = new FileInfo(backupFilePath);
            file.Delete();

            _logger.Info($"Удалён файл {file.FullName}");
        }
    }
}
