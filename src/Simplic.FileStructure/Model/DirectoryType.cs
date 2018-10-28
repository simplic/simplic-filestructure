using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type
    /// </summary>
    public class DirectoryType
    {
        /// <summary>
        /// Gets or sets the type id
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the icon name
        /// </summary>
        public string IconName
        {
            get;
            set;
        } = "directory_x16";

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether drag/drop is enabled
        /// </summary>
        public bool EnableDrag
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Gets or sets whether dropping is enabled
        /// </summary>
        public bool EnableDrop
        {
            get;
            set;
        } = true;
    }
}
