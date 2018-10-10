using Simplic.Data;
using System;

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
    }
}