using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the document workflow state provider service interface
    /// </summary>
    public interface IDocumentWorkflowStateProviderService
    {
        /// <summary>
        /// Returns the document workflow state based on a document id and a workflow id
        /// </summary>
        /// <param name="documentId"></param>
        DocumentWorkflowState GetNewDocumentWorkflowState(Guid documentId, Guid workflowId);  
    }
}
