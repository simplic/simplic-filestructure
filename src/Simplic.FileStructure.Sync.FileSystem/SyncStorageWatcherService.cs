using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Storage watcher interface
    /// </summary>
    public class SyncStorageWatcherService : ISyncStorageWatcherService
    {
        private readonly ISyncStorageService storageService;
        private readonly IFileStructureService structureService;
        private FileStructure structure;
        private IList<SyncQueueEntry> fileChanges;

        /// <summary>
        /// Initialize watcher service
        /// </summary>
        /// <param name="storageService">Storage instance</param>
        /// <param name="structureService">Structure service</param>
        public SyncStorageWatcherService(ISyncStorageService storageService, IFileStructureService structureService)
        {
            this.storageService = storageService;
            this.structureService = structureService;
            this.fileChanges = new List<SyncQueueEntry>();
        }

        /// <summary>
        /// Initialize and start watcher service
        /// </summary>
        /// <param name="structure">Structure instance</param>
        public void Initialize(FileStructure structure)
        {
            this.structure = structure;

            var watcher = new FileSystemWatcher(structure.SyncPath);

            watcher.Created += Watcher_Created;
            watcher.Renamed += Watcher_Renamed;
            watcher.Changed += Watcher_Changed;
            watcher.Deleted += Watcher_Deleted;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var entry = new SyncQueueEntry
            {
                Location = ChangeLocation.Storage,
                SourcePath = e.FullPath,
                Type = IsDirectory(e.FullPath) ? ChangeType.DeleteDirectory : ChangeType.DeleteFile
            };

            fileChanges.Add(entry);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var entry = new SyncQueueEntry
            {
                Location = ChangeLocation.Storage,
                SourcePath = e.FullPath,
                Type = IsDirectory(e.FullPath) ? ChangeType.NewDirectory : ChangeType.NewFile
            };

            fileChanges.Add(entry);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsDirectory(e.FullPath))
            {
                var entry = new SyncQueueEntry
                {
                    Location = ChangeLocation.Storage,
                    SourcePath = e.FullPath,
                    Type = ChangeType.FileChanged
                };

                fileChanges.Add(entry);
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var entry = new SyncQueueEntry
            {
                Location = ChangeLocation.Storage,
                SourcePath = e.OldFullPath,
                TargetPath = e.FullPath,
                Type = IsDirectory(e.FullPath) ? ChangeType.MoveDirectory : ChangeType.MoveFile
            };

            fileChanges.Add(entry);
        }

        /// <summary>
        /// Collect and process changes
        /// </summary>
        /// <returns>True if successfull</returns>
        public bool Collect()
        {
            var toProcess = fileChanges.Where(x => x.CreateDateTime <= DateTime.Now.AddSeconds(-10)).OrderBy(x => x.CreateDateTime)
                .ToList();

            foreach (var entry in toProcess)
                fileChanges.Remove(entry);

            // If delete and new file is in the queue (same file). It will be changed to moved
            

            // print
            foreach (var entry in toProcess)
                Console.WriteLine(entry);

            return true;
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
