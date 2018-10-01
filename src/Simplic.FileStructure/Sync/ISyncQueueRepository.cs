using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Change queue repository
    /// </summary>
    public interface ISyncQueueRepository : IRepositoryBase<Guid, SyncQueueEntry>
    {
        /// <summary>
        /// Get all unhandled changes sorted
        /// </summary>
        /// <returns>Enumerable of changes</returns>
        IEnumerable<SyncQueueEntry> GetUnhandled();
    }
}
