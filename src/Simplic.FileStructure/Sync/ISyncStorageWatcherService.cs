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
    public interface ISyncStorageWatcherService
    {
        /// <summary>
        /// Initialize and start watcher service
        /// </summary>
        /// <param name="structure">Structure instance</param>
        void Initialize(FileStructure structure);

        /// <summary>
        /// Collect and process changes
        /// </summary>
        /// <returns>True if successfull</returns>
        bool CollectAndProcess();
    }
}
