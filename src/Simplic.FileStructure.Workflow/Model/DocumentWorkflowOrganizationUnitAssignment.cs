using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the poco for the assignment between <see cref="Document"/> and <see cref="WorkflowOrganizationUnit"/>
    /// </summary>
    public class DocumentWorkflowOrganizationUnitAssignment
    {
        /// <summary>
        /// Gets or sets the guid 
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Gets or sets the workflow organization unit
        /// </summary>
        public Guid WorkflowOrganizationUnitId { get; set; }

        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        public Guid DocumentId { get; set; }
    }
}
