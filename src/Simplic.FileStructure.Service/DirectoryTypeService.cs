using System;
using System.Collections.Generic;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory type service implementation
    /// </summary>
    public class DirectoryTypeService : IDirectoryTypeService
    {
        private readonly IDirectoryTypeRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public DirectoryTypeService(IDirectoryTypeRepository repository)
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
        public bool Delete(DirectoryType obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get directory type by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory type instance</returns>
        public DirectoryType Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get all directory types
        /// </summary>
        /// <returns>Enumerable of type instance</returns>
        public IEnumerable<DirectoryType> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save the directory type
        /// </summary>
        /// <param name="obj">Directory type instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryType obj)
        {
            return repository.Save(obj);
        }
    }
}
