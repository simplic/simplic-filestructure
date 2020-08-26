using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Newtonsoft.Json;
using System.Collections.Generic;
using Dapper;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// File structure database repository
    /// </summary>
    public class FileStructureDocumentPathRepository : SqlRepositoryBase<Guid, FileStructureDocumenPath>, IFileStructureDocumentPathRepository
    {
        private JsonSerializerSettings jsonSettings;
        private ISqlService sqlService;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService"></param>
        /// <param name="sqlColumnService"></param>
        /// <param name="cacheService"></param>
        public FileStructureDocumentPathRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            this.sqlService = sqlService;
            jsonSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }


        /// <summary>
        /// Get all by document id
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <returns>Enumerable of paths</returns>
        public IEnumerable<FileStructureDocumenPath> GetByDocumentId(Guid documentId)
        {
            return GetAllByColumn<Guid>("DocumentGuid", documentId);
        }

        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        /// <param name="obj">Object id</param>
        /// <returns>Object id</returns>
        public override Guid GetId(FileStructureDocumenPath obj)
        {
            return obj.Id;
        }

        public IEnumerable<FileStructureDocumenPath> GetByDirectoryId(Guid directoryId)
        {
            return GetAllByColumn<Guid>("DirectoryGuid", directoryId);
        }

        public bool IsProtected(List<Guid> guids, SeperatedStringBuilder ssb = null)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.QueryFirst<bool>($"SELECT CASE WHEN EXISTS (" +
                    $"SELECT * FROM {TableName} where DirectoryGuid in ( {ssb} ) and IsProtectedPath = 1)" +
                    $"THEN CAST(1 AS BIT)" +
                    $"ELSE CAST(0 AS BIT) END");
            });
        }

        /// <summary>
        /// Gets the primary column key
        /// </summary>
        public override string PrimaryKeyColumn
        {
            get
            {
                return "Id";
            }
        }

        /// <summary>
        /// Gets the table name
        /// </summary>
        public override string TableName
        {
            get
            {
                return "FileStructure_DocumentPath";
            }
        }
    }
}
