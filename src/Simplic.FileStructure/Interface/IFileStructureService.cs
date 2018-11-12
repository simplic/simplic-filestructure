using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// File system structure service interface
    /// </summary>
    public interface IFileStructureService : IFileStructureRepository
    {
        /// <summary>
        /// Remove directory
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        /// <returns>True if successfull</returns>
        bool RemoveDirectory(FileStructure fileStructure, Directory directory);

        /// <summary>
        /// Move directory
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Moved directory</param>
        /// <param name="oldParent">Old parent directory</param>
        /// <param name="newParent">New parent directory</param>
        /// <returns>True if successfull</returns>
        bool MoveDirectory(FileStructure fileStructure, Directory directory, Directory oldParent, Directory newParent);
    }
}
