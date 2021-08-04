using System;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// Empty access provider, that will not set any user access information
    /// </summary>
    public class NoneDocumentWorkflowAccessProvider : IDocumentWorkflowAccessProvider
    {
        /// <summary>
        /// *nothing*
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="documentId">Unique document id</param>
        /// <param name="fileStructureId">Unique filestructure id</param>
        /// <param name="fileStructurePathId">Unique path id</param>
        /// <param name="configuration">Unique path id</param>
        /// <param name="state">Workflow state</param>
        public void SetUserAccess(int userId, Guid documentId, Guid fileStructureId, Guid fileStructurePathId, DocumentWorkflowConfiguration configuration)
        {
            // Do nothing here
        }

        /// <summary>
        /// *nothing*
        /// </summary>
        /// <param name="organizationUnitId">Organization unit id</param>
        /// <param name="documentId">Unique document id</param>
        /// <param name="fileStructureId">Unique filestructure id</param>
        /// <param name="fileStructurePathId">Unique path id</param>
        /// <param name="configuration">Unique path id</param>
        /// <param name="state">Workflow state</param>
        public void SetOrganizationUnitAcess(Guid organizationUnitId, Guid documentId, DocumentWorkflowConfiguration configuration)
        {
            // Do nothing here
        }

        /// <summary>
        /// Gets the access provider name
        /// </summary>
        public string Name => "none";
    }
}
