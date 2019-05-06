using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// represents the different directory classifications a directory type can have
    /// </summary>
    public class DirectoryTypeClassification
    {
        /// <summary>
        /// Gets or the sets the GUID
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the Directory classification Id
        /// </summary>
        public Guid DirectoryClassificationId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Directory type Id
        /// </summary>
        public Guid DirectoryTypeId
        {
            get;
            set;
        }
    }
}
