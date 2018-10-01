using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Sync queue repository implementation
    /// </summary>
    public class SyncQueueRepository : SqlRepositoryBase<Guid, SyncQueueEntry>, ISyncQueueRepository
    {
        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService"></param>
        /// <param name="sqlColumnService"></param>
        /// <param name="cacheService"></param>
        public SyncQueueRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            
        }

        /// <summary>
        /// Get all unhandled changes sorted
        /// </summary>
        /// <returns>Enumerable of changes</returns>
        public IEnumerable<SyncQueueEntry> GetUnhandled()
        {
            return GetAllByColumn("IsHandled", 0).OrderBy(x => x.CreateDateTime);
        }

        /// <summary>
        /// Gets the id of <see cref="SyncQueueEntry"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Id</returns>
        public override Guid GetId(SyncQueueEntry obj)
        {
            return obj.Id;
        }

        /// <summary>
        /// Gets the primary column key
        /// </summary>
        public override string PrimaryKeyColumn
        {
            get
            {
                return "Guid";
            }
        }

        /// <summary>
        /// Gets the table name
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_SyncQueue";
            }
        }
    }
}
