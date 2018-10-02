using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Simplic.FileStructure.Sync.FileSystem
{
    /// <summary>
    /// Sync service file system implementation
    /// </summary>
    public class SyncStorageService : ISyncStorageService
    {
        /// <summary>
        /// Esure that a path is existing
        /// </summary>
        /// <param name="path">Path to check</param>
        public void EnsurePath(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Get all subdirectories
        /// </summary>
        /// <param name="path">Root path</param>
        /// <returns>Enumerable of paths</returns>
        public IEnumerable<string> GetAllSubdirectories(string path)
        {
            return System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
        }

        /// <summary>
        /// Get all files
        /// </summary>
        /// <param name="path">Path to read all bytes from</param>
        /// <param name="includeSubdirectories">True if subdirectories should be included</param>
        /// <returns>Enumerable of file paths</returns>
        public IEnumerable<string> GetFiles(string path, bool includeSubdirectories = false)
        {
            return System.IO.Directory.GetFiles(path, "*", includeSubdirectories == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Check whether a directory exists or not
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if the directory exists</returns>
        public bool IsDirectoryExisting(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        /// <summary>
        /// Check whether a file exists or not
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if the file exists</returns>
        public bool IsFileExisting(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Read all bytes from path
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>File content</returns>
        public byte[] ReadBytes(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove directory
        /// </summary>
        /// <param name="path">Path of the directory to delete</param>
        /// <returns>True if deleteing was successfull</returns>
        public bool DeleteDirectory(string path)
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

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="path">Path of the file to delete</param>
        /// <returns>True if deleteing was successfull</returns>
        public bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                // TODO: Error message
                return false;
            }
        }

        /// <summary>
        /// Write all bytes
        /// </summary>
        /// <param name="path">Path to write to</param>
        /// <param name="bytes">Bytes to write</param>
        /// <returns>True if successfull</returns>
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

        /// <summary>
        /// Creates a hash for a directory and its content
        /// </summary>
        /// <param name="path">Directory</param>
        /// <returns>Hash as string</returns>
        public string GetDirectoryHash(string path)
        {
            if (!IsDirectoryExisting(path))
                return "";

            if (path.Length > 246)
                return $"Path-Too-Long {path}";

            var files = GetFiles(path).ToList();
            if (files.Count == 0)
                return "-";

            var hashBase = new StringBuilder();
            foreach (var file in files.OrderBy(x => x))
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    hashBase.Append(fileInfo.Name);
                    hashBase.Append(fileInfo.Length);
                    hashBase.Append(fileInfo.LastWriteTime.ToLongDateString());
                    hashBase.Append(fileInfo.LastWriteTime.ToLongTimeString());
                }
                catch (FileNotFoundException)
                {
                    return Guid.NewGuid().ToString();
                }
            }

            return Security.Cryptography.CryptographyHelper.HashSHA256(hashBase.ToString());
        }

        /// <summary>
        /// Generate file hash
        /// </summary>
        /// <param name="path">File to hash</param>
        /// <returns>Hash file</returns>
        public string GetFileHash(string path)
        {
            if (!IsFileExisting(path))
                return "";

            if (path.Length > 246)
                return $"Path-Too-Long {path}";

            try
            {
                var hashBase = new StringBuilder();
                var fileInfo = new FileInfo(path);
                hashBase.Append(fileInfo.Name);
                hashBase.Append(fileInfo.Length);
                hashBase.Append(fileInfo.LastWriteTime.ToLongDateString());
                hashBase.Append(fileInfo.LastWriteTime.ToLongTimeString());

                return Security.Cryptography.CryptographyHelper.HashSHA256(hashBase.ToString());
            }
            catch (FileNotFoundException)
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
