using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// Directory type repository implementation
    /// </summary>
    public class DirectoryTypeRepository : SqlRepositoryBase<Guid, DirectoryType>, IDirectoryTypeRepository
    {
        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        /// <param name="sqlColumnService">Sql column</param>
        /// <param name="cacheService">Cache service</param>
        public DirectoryTypeRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
        }

        /// <summary>
        /// Gets the object id of <see cref="DirectoryType"/>
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Object id</returns>
        public override Guid GetId(DirectoryType obj)
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
        /// Gets the table name (`FileStructure_DirectoryType`)
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_DirectoryType";
            }
        }
    }
}
