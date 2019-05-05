using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type repository base
    /// </summary>
    public interface IDirectoryTypeFieldRepository : IRepositoryBase<Guid, DirectoryTypeField>
    {
        IEnumerable<DirectoryTypeField> GetByDirectoryTypeId(string guid);
    }
}
