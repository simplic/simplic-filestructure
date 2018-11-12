using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// File structure database repository
    /// </summary>
    public class FileStructureRepository : SqlRepositoryBase<Guid, FileStructure>, IFileStructureRepository
    {
        private JsonSerializerSettings jsonSettings;
        private ISqlService sqlService;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService"></param>
        /// <param name="sqlColumnService"></param>
        /// <param name="cacheService"></param>
        public FileStructureRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            this.sqlService = sqlService;

            jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        /// <summary>
        /// Returns an enumerable of documents for a given directory and file structure
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        /// <param name="includeSubdirectories">True if subdirectories should be included</param>
        /// <returns>Enumerable of document guids</returns>
        public IEnumerable<Guid> GetDocuments(FileStructure fileStructure, Directory directory, bool includeSubdirectories)
        {
            if (fileStructure != null)
            {
                var path = "";
                var currentItem = fileStructure.Directories.FirstOrDefault(x => x.Id == directory.Id);
                while (currentItem != null)
                {
                    path = path.Insert(0, $"/{currentItem.Name}");

                    if (currentItem.Parent != null)
                    {
                        currentItem = currentItem.Parent;
                    }
                    else
                    {
                        currentItem = null;
                        break;
                    }
                }

                if (includeSubdirectories)
                    path = $"{path}%";

                return sqlService.OpenConnection((connection) =>
                {
                    return connection.Query<Guid>("SELECT DocumentGuid FROM FileStructure_DocumentPath WHERE FileStructureGuid = :fileStructureGuid AND Path LIKE :path",
                        new { fileStructureGuid = fileStructure.Id, path = path });
                });
            }

            return new List<Guid> { };
        }

        /// <summary>
        /// Get by column and deserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override FileStructure GetByColumn<T>(string columnName, T id)
        {
            var structure = base.GetByColumn<T>(columnName, id);

            if (structure == null)
                return null;

            return JsonConvert.DeserializeObject<FileStructure>(Encoding.UTF8.GetString(structure.Configuration), jsonSettings);
        }

        /// <summary>
        /// Get file structure by instance data guid
        /// </summary>
        /// <param name="id">Unique instance data guid</param>
        /// <returns>File structure instance if exists</returns>
        public FileStructure GetByInstanceDataGuid(Guid id)
        {
            return GetByColumn<Guid>("InstanceDataGuid", id);
        }

        /// <summary>
        /// Get all structures
        /// </summary>
        /// <returns>Structure instance</returns>
        public override IEnumerable<FileStructure> GetAll()
        {
            foreach (var structure in base.GetAll())
            {
                yield return JsonConvert.DeserializeObject<FileStructure>(Encoding.UTF8.GetString(structure.Configuration), jsonSettings);
            }
        }

        /// <summary>
        /// Get all by a specific type column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override IEnumerable<FileStructure> GetAllByColumn<T>(string columnName, T id)
        {
            foreach (var structure in base.GetAllByColumn<T>(columnName, id))
            {
                yield return JsonConvert.DeserializeObject<FileStructure>(Encoding.UTF8.GetString(structure.Configuration), jsonSettings);
            }
        }

        /// <summary>
        /// Serialize and save
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>True if saving was successfull</returns>
        public override bool Save(FileStructure obj)
        {
            obj.Configuration = Encoding.UTF8.GetBytes
                (
                    JsonConvert.SerializeObject(obj, jsonSettings)
                );

            return base.Save(obj);
        }

        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        /// <param name="obj">Object id</param>
        /// <returns>Object id</returns>
        public override Guid GetId(FileStructure obj)
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
                return "FileStructure";
            }
        }
    }
}
