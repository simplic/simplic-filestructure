using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines methods for storing the assignments of workflow organization unit in a document based workflow
    /// </summary>
    public interface IDocumentWorkflowOrganizationUnitAssignmentRepository : IRepositoryBase<Guid, DocumentWorkflowOrganizationUnitAssignment>
    {
        /// <summary>
        /// Deletes the <see cref="DocumentWorkflowOrganizationUnitAssignment"/> based on the tuple between document id and organization id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool DeleteByIds(Guid documentId, Guid organizationId);
    }
}
