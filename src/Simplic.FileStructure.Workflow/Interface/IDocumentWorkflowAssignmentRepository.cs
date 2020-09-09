using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the document workflow assignment repository interface
    /// </summary>
    public interface IDocumentWorkflowAssignmentRepository : IRepositoryBase<Guid, DocumentWorkflowAssignment>
    {
        /// <summary>
        /// Returns a bool which is true if the pair of document id and workflow id exists 
        /// </summary>
        /// <param name="documentId">Id of the document from</param>
        /// <param name="workflowId">Id of the workflow <see cref="DocumentWorkflowConfiguration"/></param>
        /// <returns>True if the tuple of document id and workflow id exists, in any other case, false</returns>
        bool Exists(Guid documentId, Guid workflowId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="workflowId"></param>
        /// <param name="stateId"></param>
        void SetState(Guid documentId, Guid workflowId, Guid stateId);
    }
}
