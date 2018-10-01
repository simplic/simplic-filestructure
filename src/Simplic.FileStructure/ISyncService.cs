using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Sync service interface
    /// </summary>
    public interface ISyncService
    {
        /// <summary>
        /// Check whether a file exists or not
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if the file exists</returns>
        bool IsFileExisting(string path);

        /// <summary>
        /// Check whether a directory exists or not
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if the directory exists</returns>
        bool IsDirectoryExisting(string path);

        /// <summary>
        /// Write all bytes
        /// </summary>
        /// <param name="path">Path to write to</param>
        /// <param name="bytes">Bytes to write</param>
        /// <returns>True if successfull</returns>
        bool WriteBytes(string path, byte[] bytes);

        /// <summary>
        /// Read all bytes from path
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>File content</returns>
        byte[] ReadBytes(string path);

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="path">Path of the file to delete</param>
        /// <returns>True if deleteing was successfull</returns>
        bool DeleteFile(string path);

        /// <summary>
        /// Remove directory
        /// </summary>
        /// <param name="path">Path of the directory to delete</param>
        /// <returns>True if deleteing was successfull</returns>
        bool DeleteDirectory(string path);

        /// <summary>
        /// Get all files
        /// </summary>
        /// <param name="path">Path to read all bytes from</param>
        /// <param name="includeSubdirectories">True if subdirectories should be included</param>
        /// <returns>Enumerable of file paths</returns>
        IEnumerable<string> GetFiles(string path, bool includeSubdirectories = false);

        /// <summary>
        /// Esure that a path is existing
        /// </summary>
        /// <param name="path">Path to check</param>
        void EnsurePath(string path);

    }
}
