using System;
using System.Collections.Generic;
using System.IO;

namespace Simplic.FileStructure.Sync.FileSystem
{
    /// <summary>
    /// Sync service file system implementation
    /// </summary>
    public class SyncService : ISyncService
    {
        public bool EnsurePath(string path)
        {
            return System.IO.Directory.CreateDirectory(path) != null;
        }

        public IEnumerable<string> GetFiles(string path, bool includeSubdirectories = false)
        {
            return System.IO.Directory.GetFiles(path, "*.*", includeSubdirectories == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public bool IsDirectoryExisting(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public bool IsFileExisting(string path)
        {
            return System.IO.File.Exists(path);
        }

        public byte[] ReadBytes(string path)
        {
            try
            {
                return System.IO.File.ReadAllBytes(path);
            }
            catch
            {
                throw;
            }
        }

        public bool RemoveDirectory(string path)
        {
            try
            {
                System.IO.Directory.Delete(path, true);
                return true;
            }
            catch
            {
                // TODO: Error message
                return false;
            }
        }

        public bool RemoveFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                return true;
            }
            catch
            {
                // TODO: Error message
                return false;
            }
        }

        public bool WriteBytes(string path, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
                return true;
            }
            catch
            {
                // TODO: Error message
                return false;
            }
        }
    }
}
