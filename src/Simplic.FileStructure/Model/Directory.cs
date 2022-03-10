using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents a directory in the file structure
    /// </summary>
    public class Directory : IEquatable<Directory>
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

        /// <summary>
        /// Gets or sets the directory type id
        /// </summary>
        public Guid DirectoryTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the directory classification
        /// </summary>
        public DirectoryClassification DirectoryClassification
        {
            get;
            set;
        }

        /// <summary>
        /// Compares two directory instances
        /// </summary>
        /// <param name="other">Object to compare with</param>
        /// <returns>True if the directories has the same guid</returns>
        public bool Equals(Directory other)
        {
            return other.Equals(this);
        }

        /// <summary>
        /// Compares an object with the directory instance
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>True if the directories has the same guid</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as Directory;
            if (other == null)
                return false;

            return other.Id == Id;
        }

        /// <summary>
        /// Get the hash code of the directory
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Gets or sets the workflow id. The id will be used to identify the workflow configuration for specific directory.
        /// </summary>
        public Guid? WorkflowId { get; set; }

        /// <summary>
        /// Gets or sets the status if this directory, is the return directory.
        /// </summary>
        public bool IsReturnDirectory { get; set; }
    }
}
