using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory type service implementation
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
        /// Delete directory type
        /// </summary>
        /// <param name="id">Directory type id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete directory type
        /// </summary>
        /// <param name="obj">Type instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(DirectoryField obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get directory type by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory type instance</returns>
        public DirectoryField Get(Guid id)
        {
            return repository.Get(id);
        }

        public DirectoryField Get(Guid directoryId, Guid fieldTypeId)
        {
            return repository.Get(directoryId, fieldTypeId);
        }

        /// <summary>
        /// Get all directory types
        /// </summary>
        /// <returns>Enumerable of type instance</returns>
        public IEnumerable<DirectoryField> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save the directory type
        /// </summary>
        /// <param name="obj">Directory type instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryField obj)
        {
            return repository.Save(obj);
        }
    }
}
