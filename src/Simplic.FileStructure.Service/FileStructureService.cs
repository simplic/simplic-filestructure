using System;
using System.Collections.Generic;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// File structure service
    /// </summary>
    public class FileStructureService : IFileStructureService
    {
        private readonly IFileStructureRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository"></param>
        public FileStructureService(IFileStructureRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get file structure by instance data guid
        /// </summary>
        /// <param name="id">Unique instance data guid</param>
        /// <returns>File structure instance if exists</returns>
        public FileStructure GetByInstanceDataGuid(Guid id)
        {
            return repository.GetByInstanceDataGuid(id);
        }

        /// <summary>
        /// Delete a structure instance
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>True if successull</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete structure by object id
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successfull</returns>
        public bool Delete(FileStructure obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get file structure instance
        /// </summary>
        /// <param name="id">Structure id</param>
        /// <returns>Structure instance of found</returns>
        public FileStructure Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Gets all file structure instance
        /// </summary>
        /// <returns>Structure instance</returns>
        public IEnumerable<FileStructure> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save file structure to database
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successfull</returns>
        public bool Save(FileStructure obj)
        {
            return repository.Save(obj);
        }

        /// <summary>
        /// Remove directory
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        /// <returns>True if successfull</returns>
        public bool RemoveDirectory(FileStructure fileStructure, Directory directory)
        {
            return true;
        }

        /// <summary>
        /// Move directory
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Moved directory</param>
        /// <param name="oldParent">Old parent directory</param>
        /// <param name="newParent">New parent directory</param>
        /// <returns></returns>
        public bool MoveDirectory(FileStructure fileStructure, Directory directory, Directory oldParent, Directory newParent)
        {
            return true;
        }
    }
}
