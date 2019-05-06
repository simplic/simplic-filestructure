using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using System.Collections.Generic;
using Dapper;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory classification repository implementation
    /// </summary>
    public class DirectoryClassificationRepository : SqlRepositoryBase<Guid, DirectoryClassification>, IDirectoryClassificationRepository
    {
        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public DirectoryClassificationRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
        }

        /// <summary>
        /// Gets the object id of <see cref="DirectoryClassification"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(DirectoryClassification obj)
        {
            return obj.Id;
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
        /// Gets the table name (`FileStructure_DirectoryClassification`)
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_DirectoryClassification";
            }
        }
    }
}
