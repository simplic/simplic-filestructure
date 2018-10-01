using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// File / directory change type
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// New file created
        /// </summary>
        NewFile = 1,

        /// <summary>
        /// File moved
        /// </summary>
        MoveFile = 2,

        /// <summary>
        /// File deleted
        /// </summary>
        DeleteFile = 3,

        /// <summary>
        /// Directory created
        /// </summary>
        NewDirectory = 4,

        /// <summary>
        /// Directory moved
        /// </summary>
        MoveDirectory = 5,

        /// <summary>
        /// Directory deleted
        /// </summary>
        DeleteDirectory = 6
    }
}
