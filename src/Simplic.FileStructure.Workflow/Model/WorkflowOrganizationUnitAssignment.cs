using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the workflow organization assignment 
    /// It connects the <see cref="DocumentWorkflowConfiguration"/> with <see cref="WorkflowOrganizationUnit"/>
    /// </summary>
    public class WorkflowOrganizationUnitAssignment
    {
        /// <summary>
        /// Gets or sets the guid that represents the primary key
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the workflow id from <see cref="DocumentWorkflowConfiguration"/>
        /// </summary>
        public Guid WorkflowId { get; set; }

        /// <summary>
        /// Gets or sets the workflow organization unit id <see cref="WorkflowOrganizationUnit"/>
        /// </summary>
        public Guid WorkflowOrganisationUnitId{ get; set; }
       
    }
}
