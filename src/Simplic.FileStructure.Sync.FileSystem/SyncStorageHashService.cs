using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Sync.FileSystem
{
    public class SyncStorageHashService : ISyncStorageHashService
    {
        private readonly ISyncStorageService storageService;
        private string rootPath;
        private IDictionary<string, string> hashs;

        public SyncStorageHashService(ISyncStorageService storageService)
        {
            this.storageService = storageService;
            hashs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void Build(string rootPath)
        {
            this.rootPath = Path.GetDirectoryName(rootPath);

            foreach (var directory in storageService.GetAllSubdirectories(rootPath))
            {
                var path = directory.Replace(rootPath, "");

                BuildDirectoryHash(path);
            }

            foreach (var file in storageService.GetFiles(rootPath, true))
            {
                var path = file.Replace(rootPath, "");

                BuildFileHash(path);
            }
        }

        public string GetDirectoryHash(string path)
        {
            if (hashs.ContainsKey(path))
                return hashs[path];

            return BuildDirectoryHash(path);
        }

        public string GetFileHash(string path)
        {
            if (hashs.ContainsKey(path))
                return hashs[path];

            return BuildFileHash(path);
        }

        public void RemoveDirectoryHash(string path)
        {
            if (hashs.ContainsKey(path))
                hashs.Remove(path);
        }

        public void RemoveFileHash(string path)
        {
            if (hashs.ContainsKey(path))
                hashs.Remove(path);
        }

        public string BuildDirectoryHash(string path)
        {
            var completePath = Path.Combine(rootPath, path);
            var hash = storageService.GetDirectoryHash(completePath);

            hashs[path] = hash;
            return hash;
        }

        public string BuildFileHash(string path)
        {
            var completePath = Path.Combine(rootPath, path);
            var hash = storageService.GetFileHash(completePath);

            hashs[path] = hash;
            return hash;
        }
    }
}
