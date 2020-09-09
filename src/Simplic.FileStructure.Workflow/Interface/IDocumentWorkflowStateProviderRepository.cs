using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public interface IDocumentWorkflowStateProviderRepository
    {
        /// <summary>
        /// Returns a bool based on the document id and the workflow id  
        /// True if the state of this document in this workflow is completed. 
        /// </summary>
        /// <param name="documentId">Guid of the document</param>
        /// <param name="workflowId">Guid of the workflow</param>
        /// <returns></returns>
        bool IsDocumentInWorkflowCompleted(Guid documentId, Guid workflowId);
    }
}
