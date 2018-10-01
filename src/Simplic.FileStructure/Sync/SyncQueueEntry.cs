using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a change in the storage/simplic system
    /// </summary>
    public class SyncQueueEntry
    {
        /// <summary>
        /// Gets or sets a unique change id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets where the change was made
        /// </summary>
        public ChangeLocation Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the change type (new, move, delete)
        /// </summary>
        public ChangeType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the create date time of the change
        /// </summary>
        public DateTime CreateDateTime
        {
            get;
            set;
        } = DateTime.Now;

        /// <summary>
        /// Gets or sets the source path (new, move, delete)
        /// </summary>
        public string SourcePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target path (move)
        /// </summary>
        public string TargetPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the change was handled
        /// </summary>
        public bool IsHandled
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Type}@{Location} `{SourcePath}` -> `{TargetPath}` from {CreateDateTime} ({Id})";
        }
    }
}
