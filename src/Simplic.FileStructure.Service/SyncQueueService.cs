using Simplic.FileStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Change queue
    /// </summary>
    public class SyncQueueService : ISyncQueueService
    {
        private readonly ISyncQueueRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository"></param>
        public SyncQueueService(ISyncQueueRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete a queue instance
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>True if successull</returns>
        public bool Delete(Guid id)
        {
            return Delete(id);
        }

        /// <summary>
        /// Delete queue by object id
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successfull</returns>
        public bool Delete(SyncQueueEntry obj)
        {
            return Delete(obj);
        }

        /// <summary>
        /// Get file queue instance
        /// </summary>
        /// <param name="id">Structure id</param>
        /// <returns>Structure instance of found</returns>
        public SyncQueueEntry Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all queue instances
        /// </summary>
        /// <returns>Structure instance</returns>
        public IEnumerable<SyncQueueEntry> GetAll()
        {
            return GetAll();
        }

        /// <summary>
        /// Get all unhandled entries
        /// </summary>
        /// <returns>Entries</returns>
        public IEnumerable<SyncQueueEntry> GetUnhandled()
        {
            return GetUnhandled();
        }

        /// <summary>
        /// Save queue instance to database
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successfull</returns>
        public bool Save(SyncQueueEntry obj)
        {
            return Save(obj);
        }
    }
}
