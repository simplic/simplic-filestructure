using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Initialize watcher service
        /// </summary>
        /// <param name="storageService">Storage instance</param>
        /// <param name="structureService">Structure service</param>
        public SyncStorageWatcherService(ISyncStorageService storageService, IFileStructureService structureService)
        {
            this.storageService = storageService;
            this.structureService = structureService;
        }

        /// <summary>
        /// Initialize and start watcher service
        /// </summary>
        /// <param name="structure">Structure instance</param>
        public void Initialize(FileStructure structure)
        {
            this.structure = structure;
        }

        /// <summary>
        /// Collect and process changes
        /// </summary>
        /// <returns>True if successfull</returns>
        public bool CollectAndProcess()
        {
            // Get structure
            structure = structureService.Get(structure.Id);

            if (structure == null)
                return false;

            // Sync simplic -> storage

            // Sync storage -> simplic

            return true;
        }
    }
}
