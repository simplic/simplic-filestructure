using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Sync.FileSystem
{
    /// <summary>
    /// Storage watcher interface
    /// </summary>
    public class SyncStorageWatcherService : ISyncStorageWatcherService
    {
        private readonly ISyncStorageService storageService;
        private readonly IFileStructureService structureService;
        private readonly ISyncStorageHashService storageHashService;
        private FileStructure structure;
        private IList<SyncQueueEntry> tempQueue;
        private object lockObj = new object();
        /// <summary>
        /// Initialize watcher service
        /// </summary>
        /// <param name="storageService">Storage instance</param>
        /// <param name="structureService">Structure service</param>
        public SyncStorageWatcherService(ISyncStorageService storageService, IFileStructureService structureService, ISyncStorageHashService storageHashService)
        {
            this.storageService = storageService;
            this.structureService = structureService;
            this.storageHashService = storageHashService;
            this.tempQueue = new List<SyncQueueEntry>();
        }

        /// <summary>
        /// Initialize and start watcher service
        /// </summary>
        /// <param name="structure">Structure instance</param>
        public void Initialize(FileStructure structure)
        {
            this.structure = structure;

            //
            if (!structure.SyncPath.EndsWith("\\"))
                structure.SyncPath = structure.SyncPath + "\\";

            storageHashService.Build(structure.SyncPath);

            var watcher = new FileSystemWatcher(structure.SyncPath);

            watcher.Created += Watcher_Created;
            watcher.Renamed += Watcher_Renamed;
            watcher.Changed += Watcher_Changed;
            watcher.Deleted += Watcher_Deleted;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        private SyncQueueEntry CreateEntry(string sourceFullPath, string targetFullPath, string sourcePath, string targetPath, ChangeType directoryChangeType, ChangeType fileChangeType)
        {
            try
            {
                var isDirectory = IsDirectory(sourceFullPath);

                var entry = new SyncQueueEntry
                {
                    Location = ChangeLocation.Storage,
                    SourcePath = sourcePath,
                    TargetPath = targetPath,
                    Type = isDirectory ? directoryChangeType : fileChangeType
                };

                if (directoryChangeType == ChangeType.DeleteDirectory || fileChangeType == ChangeType.DeleteFile)
                {
                    if (isDirectory)
                        entry.Hash = storageHashService.GetDirectoryHash(sourcePath);
                    else
                        entry.Hash = storageHashService.GetFileHash(sourcePath);
                }
                else if (directoryChangeType == ChangeType.MoveDirectory || fileChangeType == ChangeType.MoveFile)
                {
                    if (isDirectory)
                    {
                        storageHashService.RemoveDirectoryHash(sourcePath);
                        entry.Hash = storageHashService.GetDirectoryHash(targetPath);
                    }
                    else
                    {
                        storageHashService.RemoveFileHash(sourcePath);
                        entry.Hash = storageHashService.GetFileHash(targetPath);
                    }
                }
                else
                {
                    if (isDirectory)
                        entry.Hash = storageHashService.BuildDirectoryHash(sourcePath);
                    else
                        entry.Hash = storageHashService.BuildFileHash(sourcePath);
                }

                return entry;

            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            lock (lockObj)
            {
                var entry = CreateEntry(e.FullPath, "", e.Name, "", ChangeType.DeleteDirectory, ChangeType.DeleteFile);
                if (entry != null)
                    tempQueue.Add(entry);
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            lock (lockObj)
            {
                var entry = CreateEntry(e.FullPath, "", e.Name, "", ChangeType.NewDirectory, ChangeType.NewFile);
                if (entry != null)
                    tempQueue.Add(entry);
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (lockObj)
            {
                if (!IsDirectory(e.FullPath))
                {
                    // Check whether the file has changed
                    if (storageHashService.GetFileHash(e.Name) != storageHashService.BuildFileHash(e.Name))
                    {
                        var entry = CreateEntry(e.FullPath, "", e.Name, "", ChangeType.Unknown, ChangeType.FileChanged);

                        if (entry != null)
                            tempQueue.Add(entry);
                    }
                }
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            lock (lockObj)
            {
                var entry = CreateEntry(e.OldFullPath, e.FullPath, e.OldName, e.Name, ChangeType.MoveDirectory, ChangeType.MoveFile);

                if (entry != null)
                    tempQueue.Add(entry);
            }
        }

        /// <summary>
        /// Collect and process changes
        /// </summary>
        /// <returns>True if successfull</returns>
        public bool Collect()
        {
            // To prevent locks, avoid storage operations here.
            lock (lockObj)
            {
                var processableEntries = tempQueue.Where(x => x.CreateDateTime <= DateTime.Now.AddSeconds(-10)).OrderBy(x => x.CreateDateTime)
                .ToList();

                foreach (var entry in processableEntries)
                    tempQueue.Remove(entry);

                // If delete and new file is in the queue (same file). It will be changed to moved
                // Moving is always Delete -> new
                var movedFilesAndDirectories = new List<SyncQueueEntry>();
                var removableEntries = new List<SyncQueueEntry>();

                foreach (var entry in processableEntries)
                {
                    if (entry.Type == ChangeType.DeleteDirectory || entry.Type == ChangeType.DeleteFile)
                    {
                        movedFilesAndDirectories.Add(entry);
                    }
                    if (entry.Type == ChangeType.NewDirectory || entry.Type == ChangeType.NewFile)
                    {
                        // Search for delete entry
                        var deletedFileOrDirectoryEntry = movedFilesAndDirectories.FirstOrDefault(x => x.Hash == entry.Hash);
                        if (deletedFileOrDirectoryEntry != null)
                        {
                            // Switch to move
                            if (deletedFileOrDirectoryEntry.Type == ChangeType.DeleteDirectory)
                            {
                                deletedFileOrDirectoryEntry.Type = ChangeType.MoveDirectory;
                                storageHashService.RemoveDirectoryHash(deletedFileOrDirectoryEntry.SourcePath);
                                storageHashService.RemoveDirectoryHash(entry.SourcePath);
                                storageHashService.BuildDirectoryHash(entry.SourcePath);

                            }

                            if (deletedFileOrDirectoryEntry.Type == ChangeType.DeleteFile)
                            {
                                deletedFileOrDirectoryEntry.Type = ChangeType.MoveFile;
                                storageHashService.RemoveFileHash(deletedFileOrDirectoryEntry.SourcePath);
                                storageHashService.RemoveFileHash(entry.SourcePath);
                                storageHashService.BuildFileHash(entry.SourcePath);
                            }

                            deletedFileOrDirectoryEntry.TargetPath = entry.SourcePath;

                            removableEntries.Add(deletedFileOrDirectoryEntry);
                            removableEntries.Add(entry);
                        }
                    }
                }

                // Just keep moved objects
                movedFilesAndDirectories = movedFilesAndDirectories.Where(x => x.Type == ChangeType.MoveFile || x.Type == ChangeType.MoveDirectory)
                    .ToList();

                // Remove moved entries
                foreach (var entry in removableEntries)
                    processableEntries.Remove(entry);

                foreach (var entry in movedFilesAndDirectories)
                    processableEntries.Add(entry);

                // print
                foreach (var entry in processableEntries.OrderBy(x => x.CreateDateTime))
                    Console.WriteLine(entry);

                return true;
            }
        }

        /// <summary>
        /// Check whether path is directory or file
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if the path is a directory</returns>
        private bool IsDirectory(string path)
        {
            if (!File.Exists(path) && !System.IO.Directory.Exists(path))
            {
                if (string.IsNullOrWhiteSpace(Path.GetExtension(path)))
                    return true;
                else
                    return false;
            }

            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
                return true;
            else
                return false;
        }
    }
}
