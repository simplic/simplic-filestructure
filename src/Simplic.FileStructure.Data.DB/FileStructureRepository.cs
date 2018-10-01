using Simplic.Data.Sql;
using System;
using Simplic.Cache;
using Simplic.Sql;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace Simplic.FileStructure.Data.DB
{
    /// <summary>
    /// File structure database repository
    /// </summary>
    public class FileStructureRepository : SqlRepositoryBase<Guid, FileStructure>, IFileStructureRepository
    {
        private JsonSerializerSettings jsonSettings;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService"></param>
        /// <param name="sqlColumnService"></param>
        /// <param name="cacheService"></param>
        public FileStructureRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            jsonSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto
            };
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
            return JsonConvert.DeserializeObject<FileStructure>(Encoding.UTF8.GetString(structure.Configuration), jsonSettings);
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
                return "FileStructure";
            }
        }
    }
}
