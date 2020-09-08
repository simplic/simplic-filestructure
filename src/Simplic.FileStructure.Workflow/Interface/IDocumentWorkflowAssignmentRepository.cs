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
        /// Returns a bool which is true if the pair of document id and workflow id already exists 
        /// </summary>
        /// <param name="documentId"> The id of the document</param>
        /// <returns>a bool true if the id already exists</returns>
        bool AlreadyExists(Guid documentId, Guid workflowId);
    }
}
