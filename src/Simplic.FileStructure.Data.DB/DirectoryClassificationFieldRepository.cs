using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Dapper;
using System.Collections.Generic;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory classification to field repository implementation
    /// </summary>
    public class DirectoryClassificationFieldRepository : SqlRepositoryBase<Guid, DirectoryClassificationField>, IDirectoryClassificationFieldRepository
    {
        private readonly ISqlService sqlService;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public DirectoryClassificationFieldRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Gets the object id of <see cref="DirectoryClassificationField"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(DirectoryClassificationField obj)
        {
            return obj.Id;
        }

        /// <summary>
        /// Returns all field types for a given DirectoryClassificationId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<DirectoryClassificationField> GetByDirectoryClassificationId(Guid guid)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<DirectoryClassificationField>($"SELECT * FROM {TableName} WHERE DirectoryClassificationId = :guid",
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
        /// Gets the table name (`FileStructure_DirectoryClassificationField`)
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_DirectoryClassificationField";
            }
        }
    }
}
