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
        bool IsFileExisting(string path);
        bool IsDirectoryExisting(string path);
        bool WriteBytes(string path, byte[] bytes);
        byte[] ReadBytes(string path);
        bool RemoveFile(string path);
        bool RemoveDirectory(string path);
        IEnumerable<string> GetFiles(string path, bool includeSubdirectories = false);
        bool EnsurePath(string path);

    }
}
