using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Dapper;
using System.Collections.Generic;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory type repository implementation
    /// </summary>
    public class DirectoryTypeFieldRepository : SqlRepositoryBase<Guid, DirectoryTypeField>, IDirectoryTypeFieldRepository
    {
        private readonly ISqlService sqlService;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public DirectoryTypeFieldRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Gets the object id of <see cref="DirectoryTypeField"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(DirectoryTypeField obj)
        {
            return obj.Id;
        }

        public IEnumerable<DirectoryTypeField> GetByDirectoryTypeId(string guid)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<DirectoryTypeField>($"SELECT * FROM {TableName} WHERE DirectoryTypeId = :guid",
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
        /// Gets the table name (`FileStructure_DirectoryTypeField`)
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_DirectoryTypeField";
            }
        }
    }
}
