using Simplic.FileStructure.Workflow;
using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Tracking object for <see cref="FileStructureDocumenPath"/>
    /// </summary>
    public class FileStructureDocumenPathTracking : FileStructureDocumenPath
    {
        /// <summary>
        /// Gets or sets the current user id
        /// </summary>
        public int UserId { get; set; }
    }
}
