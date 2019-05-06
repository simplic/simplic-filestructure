using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory to field repository base
    /// </summary>
    public interface IDirectoryFieldRepository : IRepositoryBase<Guid, DirectoryField>
    {
        /// <summary>
        /// Get the value of a field by directoryId and fieldTypeId
        /// </summary>
        /// <param name="directoryId">Id of a directory</param>
        /// <param name="fieldTypeId">Id of a field type</param>
        /// <returns><see cref="DirectoryField"/></returns>
        DirectoryField Get(Guid directoryId, Guid fieldTypeId);
    }
}
