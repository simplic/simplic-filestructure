using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a file structure
    /// </summary>
    public class FileStructure
    {
        /// <summary>
        /// Gets or sets the structure id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets whether to use file system sync or not
        /// </summary>
        public bool UseFileSync
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current hash for sync
        /// </summary>
        public string SyncHash
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the server path for file sync
        /// </summary>
        public string SyncPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the structure name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this is a structure template
        /// </summary>
        public bool IsTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a list of directories
        /// </summary>
        public IList<Directory> Directories
        {
            get;
            set;
        } = new List<Directory>();

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        [JsonIgnore]
        public byte[] Configuration
        {
            get;
            set;
        } 
    }
}
