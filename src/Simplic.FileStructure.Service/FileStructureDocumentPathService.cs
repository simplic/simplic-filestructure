using Simplic.FileStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// File structure service
    /// </summary>
    public class FileStructureDocumentPathService : IFileStructureDocumentPathService
    {
        private readonly IFileStructureDocumentPathRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository"></param>
        public FileStructureDocumentPathService(IFileStructureDocumentPathRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete a path instance
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>True if successull</returns>
        public bool Delete(Guid id)
        {
            return Delete(id);
        }

        /// <summary>
        /// Delete path by object id
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successfull</returns>
        public bool Delete(FileStructureDocumenPath obj)
        {
            return Delete(obj);
        }

        /// <summary>
        /// Get file path instance
        /// </summary>
        /// <param name="id">Structure id</param>
        /// <returns>Structure instance of found</returns>
        public FileStructureDocumenPath Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all file structure instance
        /// </summary>
        /// <returns>Structure instance</returns>
        public IEnumerable<FileStructureDocumenPath> GetAll()
        {
            return GetAll();
        }

        /// <summary>
        /// Save file structure to database
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successfull</returns>
        public bool Save(FileStructureDocumenPath obj)
        {
            return Save(obj);
        }
    }
}
