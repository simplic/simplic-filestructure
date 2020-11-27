using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines method for managing the <see cref="WorkflowOperation"/>
    /// </summary>
    public interface IWorkflowOperationService 

    {
        /// <summary>
        /// Fowards the WorkflowOperation to all user that have an instance of the work flow
        /// </summary>
        void ForwardTo(WorkflowOperation workflowOperation);

        /// <summary>
        /// Fowards a copy WorkflowOperation to all user that have an instance of the work flow
        /// </summary>
        void ForwardCopyTo(WorkflowOperation workflowOperation);

        /// <summary>
        /// Checks a document out of the <see cref="WorkflowOrganizationUnit"/> and assigns it to the user
        /// </summary>
        void DocumentCheckout(WorkflowOperation workflowOperation);

        /// <summary>
        /// Sets the state of the document to complete
        /// </summary>
        void Complete(WorkflowOperation workflowOperation);
    }
}
