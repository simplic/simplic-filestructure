using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory type repository implementation
    /// </summary>
    public class DirectoryFieldRepository : SqlRepositoryBase<Guid, DirectoryField>, IDirectoryFieldRepository
    {
        private readonly ISqlService sqlService;
        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public DirectoryFieldRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Gets the object id of <see cref="DirectoryTypeField"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(DirectoryField obj)
        {
            return obj.Id;
        }

        public DirectoryField Get(Guid directoryId, Guid fieldTypeId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<DirectoryField>($"SELECT * FROM {TableName} WHERE FieldTypeId = :fieldTypeId AND DirectoryId = :directoryId",
                    new { fieldTypeId, directoryId }).FirstOrDefault();
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
                return "FileStructure_DirectoryField";
            }
        }
    }
}
