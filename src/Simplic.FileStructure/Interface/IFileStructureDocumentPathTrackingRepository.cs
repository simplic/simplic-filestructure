using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// File structure repository
    /// </summary>
    public interface IFileStructureDocumentPathTrackingRepository : IRepositoryBase<Guid, FileStructureDocumenPathTracking>
    {
        /// <summary>
        /// Get all by document id
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <returns>Enumerable of paths</returns>
        IEnumerable<FileStructureDocumenPath> GetByDocumentId(Guid documentId);
    }
}
