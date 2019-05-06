using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory classification to field service
    /// </summary>
    public interface IDirectoryClassificationFieldService : IDirectoryClassificationFieldRepository
    {
        /// <summary>
        /// Saves an Array of allowed fieldtypes for a given DirectoryClassification
        /// </summary>
        /// <param name="fieldTypes">The allowed field types</param>
        /// <param name="notChosenFieldTypes">The not allowed field types</param>
        /// <param name="directoryClassification">The directory classification for which the field types are meant for</param>
        /// <returns></returns>
        bool SaveFieldTypes(List<FieldType> fieldTypes, List<FieldType> notChosenfieldTypes, DirectoryClassification directoryClassification);

        /// <summary>
        /// Deletes all entries for a given DirectoryClassification
        /// </summary>
        /// <param name="directoryClassification"></param>
        /// <returns></returns>
        bool DeleteAll(DirectoryClassification directoryClassification);
    }
}
