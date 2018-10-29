using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// File structure database repository
    /// </summary>
    public class FileStructureDocumentPathRepository : SqlRepositoryBase<Guid, FileStructureDocumenPath>, IFileStructureDocumentPathRepository
    {
        private JsonSerializerSettings jsonSettings;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService"></param>
        /// <param name="sqlColumnService"></param>
        /// <param name="cacheService"></param>
        public FileStructureDocumentPathRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
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
                return "FileStructure_DocumentPath";
            }
        }
    }
}
