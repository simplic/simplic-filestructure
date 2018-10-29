using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// File structure service
    /// </summary>
    public class FileStructureDocumentPathService : IFileStructureDocumentPathService
    {
        private readonly IFileStructureDocumentPathRepository repository;
        private readonly IFileStructureService structureService;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="structureService">File structure service</param>
        public FileStructureDocumentPathService(IFileStructureDocumentPathRepository repository, IFileStructureService structureService)
        {
            this.repository = repository;
            this.structureService = structureService;
        }

        /// <summary>
        /// Delete a path instance
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>True if successull</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete path by object id
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successfull</returns>
        public bool Delete(FileStructureDocumenPath obj)
        {
            return repository.Delete(obj);
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
            return repository.GetAll();
        }

        /// <summary>
        /// Save file structure to database
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successfull</returns>
        public bool Save(FileStructureDocumenPath obj)
        {
            var fileStructure = structureService.Get(obj.FileStructureGuid);
            obj.Path = "";

            if (fileStructure != null)
            {
                var currentItem = fileStructure.Directories.FirstOrDefault(x => x.Id == obj.DirectoryGuid);
                while (currentItem != null)
                {
                    obj.Path = obj.Path.Insert(0, $"/{currentItem.Name}");

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
            }

            return repository.Save(obj);
        }

        /// <summary>
        /// Get all by document id
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <returns>Enumerable of paths</returns>
        public IEnumerable<FileStructureDocumenPath> GetByDocumentId(Guid documentId)
        {
            return repository.GetByDocumentId(documentId);
        }
    }
}
