using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    public interface ISyncStorageHashService
    {
        void Build(string rootPath);

        string GetFileHash(string path);
        string GetDirectoryHash(string path);
        string BuildFileHash(string path);
        string BuildDirectoryHash(string path);
        void RemoveFileHash(string path);
        void RemoveDirectoryHash(string path);
    }
}
