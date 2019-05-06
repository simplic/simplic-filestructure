using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type to directory classification service
    /// </summary>
    public interface IDirectoryTypeClassificationService : IDirectoryTypeClassificationRepository
    {
        /// <summary>
        /// Saves an Array of directory classifications for a given directory
        /// </summary>
        /// <param name="directoryClassifications"></param>
        /// <param name="notChosenDirectoryClassifications"></param>
        /// <param name="directoryType"></param>
        /// <returns></returns>
        bool SaveFieldTypes(List<DirectoryClassification> directoryClassifications, List<DirectoryClassification> notChosenDirectoryClassifications, DirectoryType directoryType);

        /// <summary>
        /// Deletes all entries for a given directory type
        /// </summary>
        /// <param name="directoryType"></param>
        /// <returns></returns>
        bool DeleteAll(DirectoryType directoryType);
    }
}
