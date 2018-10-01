using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a document path and connect it with a directory/file structure
    /// </summary>
    public class FileStructureDocumenPath
    {
        /// <summary>
        /// Gets or set the entry id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the directory guid
        /// </summary>
        public Guid DirectoryGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file structure id
        /// </summary>
        public Guid FileStructureGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        public Guid DocumentGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path as string
        /// </summary>
        public string Path
        {
            get;
            set;
        }
    }
}
