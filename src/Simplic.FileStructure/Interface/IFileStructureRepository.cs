using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// File system repository interface
    /// </summary>
    public interface IFileStructureRepository : IRepositoryBase<Guid, FileStructure>
    {
        /// <summary>
        /// Get file structure by instance data guid
        /// </summary>
        /// <param name="id">Unique instance data guid</param>
        /// <returns>File structure instance if exists</returns>
        FileStructure GetByInstanceDataGuid(Guid id);
        
        /// <summary>
        /// Returns an enumerable of documents for a given directory and file structure
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        /// <param name="includeSubdirectories">True if subdirectories should be included</param>
        /// <returns>Enumerable of document guids</returns>
        IEnumerable<Guid> GetDocuments(FileStructure fileStructure, Directory directory, bool includeSubdirectories);
    }
}