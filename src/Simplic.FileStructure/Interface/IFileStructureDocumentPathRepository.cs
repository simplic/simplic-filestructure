using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// File structure repository
    /// </summary>
    public interface IFileStructureDocumentPathRepository : IRepositoryBase<Guid, FileStructureDocumenPath>
    {
        /// <summary>
        /// Get all by document id
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <returns>Enumerable of paths</returns>
        IEnumerable<FileStructureDocumenPath> GetByDocumentId(Guid documentId);

        /// <summary>
        /// Gets all by Directory id 
        /// </summary>
        /// <param name="directoryId">Directory instance id</param>
        /// <returns>Enumerable of paths</returns>
        IEnumerable<FileStructureDocumenPath> GetByDirectoryId(Guid directoryId);

        /// <summary>
        /// Goes through a list of directory ids and checks, whether any directory contains a document, that is protected. If a protected documents exists,
        /// true will be returned. If no protected directory was found, false will be returned
        /// </summary>
        /// <param name="directoryIds">Ids for directory</param>
        /// <returns>bool</returns>
        bool IsProtected(IList<Guid> directoryIds);

    }
}
