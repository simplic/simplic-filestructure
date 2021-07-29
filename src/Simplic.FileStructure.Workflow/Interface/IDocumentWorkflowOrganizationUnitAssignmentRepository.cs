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

        /// <summary>
        /// Gets a <see cref="DocumentWorkflowOrganizationUnitAssignment"/> based on the tuple between document id and organization id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        DocumentWorkflowOrganizationUnitAssignment GetByIds(Guid documentId, Guid organizationId);

        /// Gets an enumerable of the model based on a document id and a user id
        /// <para>
        /// It is primarly used for the workflow operation service checkout method
        /// Look at <see cref="IWorkflowOperationService.DocumentCheckout(WorkflowOperation)"/> for more information
        /// </para>
        /// </summary>
        /// <param name="documentId">An unique identifier for a document</param>
        /// <param name="userId">An identifier that represents the key of the user in the database</param>
        /// <returns></returns>
        IEnumerable<DocumentWorkflowOrganizationUnitAssignment> GetByIds(Guid documentId, long userId);

    }
}
