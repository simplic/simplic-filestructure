using Simplic.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// This implementation of <see cref="IDocumentWorkflowAccessProvider"/> adds user and organization unit access
    /// for any workflow step the was executed
    /// </summary>
    public class DefaultDocumentWorkflowAccessProvider : IDocumentWorkflowAccessProvider
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IWorkflowOrganizationUnitUserAssignmentRepository workflowOrganizationUnitUserAssignmentRepository;

        /// <summary>
        /// Initialize default document workflow access provider
        /// </summary>
        /// <param name="authorizationService">Authorization service</param>
        /// <param name="workflowOrganizationUnitService">Organization unit service</param>
        public DefaultDocumentWorkflowAccessProvider(IAuthorizationService authorizationService, IWorkflowOrganizationUnitUserAssignmentRepository workflowOrganizationUnitUserAssignmentRepository)
        {
            this.authorizationService = authorizationService;
            this.workflowOrganizationUnitUserAssignmentRepository = workflowOrganizationUnitUserAssignmentRepository;
        }

        /// <summary>
        /// Adds access to the given user and document
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="documentId">Unique document id</param>
        /// <param name="fileStructureId">Unique filestructure id</param>
        /// <param name="fileStructurePathId">Unique path id</param>
        /// <param name="configuration">Unique path id</param>
        /// <param name="state">Workflow state</param>
        public void SetUserAccess(int userId, Guid documentId, Guid fileStructureId, Guid fileStructurePathId, DocumentWorkflowConfiguration configuration)
        {
            bool accessChanged = false;
            var access = authorizationService.GetAccessRights("IT_Document", "Guid", documentId);
            if (access == null)
                return;

            if (access.OwnerId == null)
            {
                access.OwnerId = userId;
                accessChanged = true;
            }

            if (access.UserFullAccess == null)
                access.UserFullAccess = new List<int>();

            if (!access.UserFullAccess.Contains(userId))
            {
                access.UserFullAccess.Add(userId);
                accessChanged = true;
            }

            if (accessChanged)
                authorizationService.SetAccess("IT_Document", "Guid", documentId, access);
        }

        /// <summary>
        /// Adds access to the all users for the given document
        /// </summary>
        /// <param name="organizationUnitId">Organization unit id</param>
        /// <param name="documentId">Unique document id</param>
        /// <param name="fileStructureId">Unique filestructure id</param>
        /// <param name="fileStructurePathId">Unique path id</param>
        /// <param name="configuration">Unique path id</param>
        /// <param name="organizationUnitId">Workflow state</param>
        public void SetOrganizationUnitAcess(Guid organizationUnitId, Guid documentId, DocumentWorkflowConfiguration configuration)
        {
            bool accessChanged = false;

            var userAssignments = workflowOrganizationUnitUserAssignmentRepository.GetForOrganizationByConfigurationId(configuration.Guid, organizationUnitId);
            var access = authorizationService.GetAccessRights("IT_Document", "Guid", documentId);

            if (access == null)
                return;

            if (userAssignments == null || !userAssignments.Any())
                return;

            if (access.UserFullAccess == null)
                access.UserFullAccess = new List<int>();

            foreach (var assignment in userAssignments)
            {
                if (!access.UserFullAccess.Contains(assignment.UserId))
                {
                    access.UserFullAccess.Add(assignment.UserId);
                    accessChanged = true;
                }
            }

            if (accessChanged)
                authorizationService.SetAccess("IT_Document", "Guid", documentId, access);
        }

        /// <summary>
        /// Gets the access provider name
        /// </summary>
        public string Name => "default";
    }
}
