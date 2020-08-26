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
        /// <param name="documentId"></param>
        /// <returns></returns>
        IEnumerable<FileStructureDocumenPath> GetByDirectoryId(Guid directoryId);

        /// <summary>
        /// Gets a bool if any of the directory is protected 
        /// </summary>
        /// <param name="guids"></param>
        /// <param name="ssb"></param>
        /// <returns></returns>
        bool IsProtected(List<Guid> guids, SeperatedStringBuilder ssb = null);

    }
}
