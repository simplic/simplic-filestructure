using Simplic.FileStructure.Model;
using System;
using System.Collections.Generic;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Directory type
    /// </summary>
    public class DirectoryType
    {
        private static Guid defaultIconGuid = Guid.Parse("CF70E2CA-32D2-4E5C-9C11-CDF740AC03DA");

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
        public Guid IconId
        {
            get;
            set;
        } = defaultIconGuid;

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type category
        /// </summary>
        public string Category
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

        /// <summary>
        /// Gets or sets the grid name
        /// </summary>
        public string GridName { get; set; } = "";

        /// <summary>
        /// Gets or sets the directory function
        /// </summary>
        public DirectoryFunctionType DirectoryFunction { get; set; } = DirectoryFunctionType.Default;
    }
}
