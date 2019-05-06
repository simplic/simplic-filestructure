using System;
using System.Collections.Generic;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory classification service implementation
    /// </summary>
    public class DirectoryClassificationService : IDirectoryClassificationService
    {
        private readonly IDirectoryClassificationRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public DirectoryClassificationService(IDirectoryClassificationRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete directory classification
        /// </summary>
        /// <param name="id">Directory classification id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete Directory classification
        /// </summary>
        /// <param name="obj">Classification instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(DirectoryClassification obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get directory classification by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory classification instance</returns>
        public DirectoryClassification Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get all directory classifications
        /// </summary>
        /// <returns>Enumerable of type instance</returns>
        public IEnumerable<DirectoryClassification> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save the Directory classification
        /// </summary>
        /// <param name="obj">Directory classification instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryClassification obj)
        {
            return repository.Save(obj);
        }

    }
}
