using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Collections.Generic;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the workflow organisation unit
    /// </summary>
    public class WorkflowOrganizationUnit
    {
        /// <summary>
        /// Gets or sets the guid 
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the internal name
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the is deleted 
        /// Returs false if this poco is not deleted, and true if it is deleted
        /// Purpose: Its just a flag, so we dont need to delete it physically from the storage system
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
