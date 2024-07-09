using System;
using System.IO;
using System.IO.Compression;
using NLog;

namespace T034.Core.Api
{
    public interface IBackupService
    {
        /// <summary>
        /// Сделать бекап
        /// </summary>
        /// <returns></returns>
        void MakeBackup();
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
                        AddDirectoryToArchive(archive, _contentRootPath, "_backups");
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
        /// <param name="excludeDir">Исключаемая папка</param>
        private static void AddDirectoryToArchive(ZipArchive archive, string sourceDir, string excludeDir)
        {
            var dir = new DirectoryInfo(sourceDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string entryName = Path.GetRelativePath(sourceDir, file.FullName);
                archive.CreateEntryFromFile(file.FullName, entryName);
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (subDir.Name.Equals(excludeDir, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                AddDirectoryToArchive(archive, subDir.FullName, excludeDir);
            }
        }
    }
}
