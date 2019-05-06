using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Dapper;
using System.Collections.Generic;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory type to directory classification repository implementation
    /// </summary>
    public class DirectoryTypeClassificationRepository : SqlRepositoryBase<Guid, DirectoryTypeClassification>, IDirectoryTypeClassificationRepository
    {
        private readonly ISqlService sqlService;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public DirectoryTypeClassificationRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Gets the object id of <see cref="DirectoryTypeClassification"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(DirectoryTypeClassification obj)
        {
            return obj.Id;
        }

        /// <summary>
        /// Get all by DirectoryTypeId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A IEnumrable of <see cref="DirectoryTypeClassification"/></returns>
        public IEnumerable<DirectoryTypeClassification> GetByDirectoryTypeId(Guid guid)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<DirectoryTypeClassification>($"SELECT * FROM {TableName} WHERE DirectoryTypeId = :guid",
                    new { guid });
            });
        }

        /// <summary>
        /// Gets the primary column name (`Id`)
        /// </summary>
        public override string PrimaryKeyColumn
        {
            get
            {
                return "Id";
            }
        }

        /// <summary>
        /// Gets the table name (`FileStructure_DirectoryTypeClassification`)
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_DirectoryTypeClassification";
            }
        }
    }
}
