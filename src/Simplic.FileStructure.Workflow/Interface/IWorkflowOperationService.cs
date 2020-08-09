using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
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
        /// Sets the state of the document to complete
        /// </summary>
        void Complete(WorkflowOperation workflowOperation);
    }
}
