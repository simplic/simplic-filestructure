using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type service
    /// </summary>
    public interface IDirectoryFieldService : IDirectoryFieldRepository
    {
        /// <summary>
        /// Saves an Array of fieldtypes for a given directory
        /// </summary>
        /// <param name="FieldTypes"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        //bool SaveFieldValues(List<FieldType> FieldTypes, DirectoryType directoryType);
    }
}
