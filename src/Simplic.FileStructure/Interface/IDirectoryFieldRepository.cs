using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type repository base
    /// </summary>
    public interface IDirectoryFieldRepository : IRepositoryBase<Guid, DirectoryField>
    {
        DirectoryField Get(Guid directoryId, Guid fieldTypeId);
    }
}
