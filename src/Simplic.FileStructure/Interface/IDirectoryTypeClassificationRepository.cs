using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type to directory classification repository base
    /// </summary>
    public interface IDirectoryTypeClassificationRepository : IRepositoryBase<Guid, DirectoryTypeClassification>
    {
        /// <summary>
        /// Get all by DirectoryTypeId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A IEnumrable of <see cref="DirectoryTypeClassification"/></returns>
        IEnumerable<DirectoryTypeClassification> GetByDirectoryTypeId(Guid guid);
    }
}
