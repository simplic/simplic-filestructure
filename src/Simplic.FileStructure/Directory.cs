using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a directory in the file structure
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// Gets or sets the directory name
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the parent directories
        /// </summary>
        public Directory Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the directory name
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
