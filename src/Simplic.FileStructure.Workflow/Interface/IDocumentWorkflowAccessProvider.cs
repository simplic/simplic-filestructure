using System;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Interface for managing document access within the simplic document workflow
    /// </summary>
    public interface IDocumentWorkflowAccessProvider
    {
        /// <summary>
        /// Set document access
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="documentId">Unique document id</param>
        /// <param name="fileStructureId">Unique filestructure id</param>
        /// <param name="fileStructurePathId">Unique path id</param>
        /// <param name="configuration">Unique path id</param>
        void SetUserAccess(int userId, Guid documentId, Guid fileStructureId, Guid fileStructurePathId, DocumentWorkflowConfiguration configuration);

        /// <summary>
        /// Set document access
        /// </summary>
        /// <param name="organizationUnitId">Organization unit id</param>
        /// <param name="documentId">Unique document id</param>
        /// <param name="fileStructureId">Unique filestructure id</param>
        /// <param name="fileStructurePathId">Unique path id</param>
        /// <param name="configuration">Unique path id</param>
        void SetOrganizationUnitAcess(Guid organizationUnitId, Guid documentId, DocumentWorkflowConfiguration configuration);

        /// <summary>
        /// Gets the access provider name
        /// </summary>
        string Name { get; }
    }
}
