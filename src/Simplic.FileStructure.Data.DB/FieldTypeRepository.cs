using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using System.Collections.Generic;
using Dapper;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory type repository implementation
    /// </summary>
    public class FieldTypeRepository : SqlRepositoryBase<Guid, FieldType>, IFieldTypeRepository
    {
        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public FieldTypeRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
        }

        /// <summary>
        /// Gets the object id of <see cref="FieldType"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(FieldType obj)
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
        /// Gets the table name (`FileStructure_FieldType`)
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_FieldType";
            }
        }
    }
}
