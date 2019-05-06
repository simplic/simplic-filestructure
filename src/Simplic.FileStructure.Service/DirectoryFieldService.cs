using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory to Field service implementation
    /// </summary>
    public class DirectoryFieldService : IDirectoryFieldService
    {
        private readonly IDirectoryFieldRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public DirectoryFieldService(IDirectoryFieldRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete directory field
        /// </summary>
        /// <param name="id">Directory field id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete directory field
        /// </summary>
        /// <param name="obj">Directory field instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(DirectoryField obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get directory field by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory type instance</returns>
        public DirectoryField Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get the value of a field by directoryId and fieldTypeId
        /// </summary>
        /// <param name="directoryId">Id of a directory</param>
        /// <param name="fieldTypeId">Id of a field type</param>
        /// <returns><see cref="DirectoryField"/></returns>
        public DirectoryField Get(Guid directoryId, Guid fieldTypeId)
        {
            return repository.Get(directoryId, fieldTypeId);
        }

        /// <summary>
        /// Get all directory fields
        /// </summary>
        /// <returns>Enumerable of DirectoryField instance</returns>
        public IEnumerable<DirectoryField> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save the directory field
        /// </summary>
        /// <param name="obj">Directory field instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryField obj)
        {
            return repository.Save(obj);
        }
    }
}
