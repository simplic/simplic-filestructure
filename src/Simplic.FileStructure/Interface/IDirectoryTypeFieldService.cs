using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type service
    /// </summary>
    public interface IDirectoryTypeFieldService : IDirectoryTypeFieldRepository
    {
        /// <summary>
        /// Saves an Array of fieldtypes for a given directory
        /// </summary>
        /// <param name="fieldTypes"></param>
        /// <param name="notChosenFieldTypes"></param>
        /// <param name="directoryType"></param>
        /// <returns></returns>
        bool SaveFieldTypes(List<FieldType> fieldTypes, List<FieldType> notChosenFieldTypes, DirectoryType directoryType);

        /// <summary>
        /// Deletes all entries for a given directory
        /// </summary>
        /// <param name="directoryType"></param>
        /// <returns></returns>
        bool DeleteAll(DirectoryType directoryType);
    }
}
