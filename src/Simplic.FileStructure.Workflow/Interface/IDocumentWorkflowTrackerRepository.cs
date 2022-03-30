using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines the method for storing the tracking for document based workflows
    /// </summary>
    public interface IDocumentWorkflowTrackerRepository : IRepositoryBase<Guid, DocumentWorkflowTracker>
    {
        /// <summary>
        /// Gets a bool value if the user has already sent this document within the specific workflow.
        /// </summary>
        /// <param name="documentId">The document id.</param>
        /// <param name="workflowId">The workflow id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        bool IsDocumentUserAssigned(Guid documentId, int userId);
    }
}
