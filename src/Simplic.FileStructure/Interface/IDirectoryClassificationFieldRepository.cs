using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory classification to field repository base
    /// </summary>
    public interface IDirectoryClassificationFieldRepository : IRepositoryBase<Guid, DirectoryClassificationField>
    {
        /// <summary>
        /// Returns all field types for a given DirectoryClassificationId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        IEnumerable<DirectoryClassificationField> GetByDirectoryClassificationId(Guid guid);
    }
}
