using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IWorkflowOperationService"/>
    /// </summary>
    public class WorkflowOperationService : IWorkflowOperationService
    {
        private readonly IUnityContainer unityContainer;
        private readonly IFileStructureService fileStructureService;
        private readonly IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService;
        private readonly IDocumentWorkflowUserService documentWorkflowUserService;
        private readonly IFileStructureDocumentPathService fileStructureDocumentPathService;
        private readonly IDocumentWorkflowTrackerService documentWorkflowTrackerService;
        private readonly IDocumentWorkflowOrganizationUnitAssignmentService documentWorkflowOrganizationUnitAssignmentService;
        private readonly IDocumentWorkflowConfigurationService documentWorkflowConfigurationService;
        private readonly IDocumentWorkflowAssignmentService documentWorkflowAssignmentService;
        /// <summary>
        /// Constructor for dependency injection 
        /// </summary>
        /// <param name="fileStructureService"></param>
        /// <param name="documentWorkflowAppSettingsService"></param>
        /// <param name="documentWorkflowUserService"></param>
        /// <param name="fileStructureDocumentPathService"></param>
        /// <param name="documentWorkflowTrackerService"></param>
        /// <param name="documentWorkflowOrganizationUnitAssignmentService"></param>
        public WorkflowOperationService(IFileStructureService fileStructureService,
                                        IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService,
                                        IDocumentWorkflowUserService documentWorkflowUserService,
                                        IFileStructureDocumentPathService fileStructureDocumentPathService,
                                        IDocumentWorkflowTrackerService documentWorkflowTrackerService,
                                        IDocumentWorkflowOrganizationUnitAssignmentService documentWorkflowOrganizationUnitAssignmentService,
                                        IDocumentWorkflowConfigurationService documentWorkflowConfigurationService,
                                        IUnityContainer unityContainer,
                                        IDocumentWorkflowAssignmentService documentWorkflowAssignmentService)
        {
            this.fileStructureService = fileStructureService;
            this.documentWorkflowAppSettingsService = documentWorkflowAppSettingsService;
            this.documentWorkflowUserService = documentWorkflowUserService;
            this.fileStructureDocumentPathService = fileStructureDocumentPathService;
            this.documentWorkflowTrackerService = documentWorkflowTrackerService;
            this.documentWorkflowOrganizationUnitAssignmentService = documentWorkflowOrganizationUnitAssignmentService;
            this.unityContainer = unityContainer;
            this.documentWorkflowConfigurationService = documentWorkflowConfigurationService;
            this.documentWorkflowAssignmentService = documentWorkflowAssignmentService;
        }

        private Directory FindWorkflowDirectory(FileStructure fileStructure, Guid workflowId)
        {
            foreach (var directory in fileStructure.Directories)
                if (directory.WorkflowId == workflowId)
                    return directory;

            // TODO: Add exception handling, a workflow is not configurated/existing, if no directory was
            // found for the specific user instance
            return null;
        }
        private void TrackChanges(WorkflowOperation workflowOperation)
        {
            DocumentWorkflowUser workflow = null;
            if (workflowOperation.WorkflowOrganizationId != null)
            {
                var tracker = new DocumentWorkflowTracker
                {
                    ActionName = DocumentWorkflowStateType.Forwarded,
                    CreateDateTime = DateTime.Now,
                    DocumentId = workflowOperation.DocumentId,
                    TargetUserId = workflowOperation.TargetUserId,
                    UserId = workflowOperation.UserId,
                    WorkflowOrganizationId = workflowOperation.WorkflowOrganizationId,
                };

                documentWorkflowTrackerService.Save(tracker);
            }
        }


        /// <summary>
        /// Sends the document to the target user id user
        /// </summary>
        /// <param name="workflowOperation"></param>
        public void ForwardTo(WorkflowOperation workflowOperation)
        {
            var configuration = documentWorkflowConfigurationService.Get(workflowOperation.WorkflowId);
            var accessProvider = unityContainer.Resolve<IDocumentWorkflowAccessProvider>(configuration.AccessProviderName);

            if (workflowOperation.OperationType == WorkflowOperationType.User)
            {
                // Add path to forwarded user
                var workflow = documentWorkflowUserService.Get(workflowOperation.TargetUserId);

                if (workflow == null)
                {
                    throw new DocumentWorkflowException("workflow is null");
                }

                var targetStructure = fileStructureService.GetByInstanceDataGuid(workflow.Guid);

                if (targetStructure == null)
                {
                    throw new DocumentWorkflowException("targetStructure is null");
                }

                var existingStructures = fileStructureDocumentPathService.GetByDocumentId(workflowOperation.DocumentId)
                                                                         .ToList();

                if (existingStructures == null)
                {
                    throw new DocumentWorkflowException("existingStructures is null");
                }

                var targetPath = existingStructures.FirstOrDefault(x => x.FileStructureGuid == targetStructure.Id);

                if (targetPath != null)
                {
                    targetPath.WorkflowState = DocumentWorkflowStateType.InReview;
                }
                else
                {
                    var firstDirectory = FindWorkflowDirectory(targetStructure, workflowOperation.WorkflowId);

                    if (firstDirectory == null)
                    {
                        throw new DocumentWorkflowException("existingStructures is null");
                    }

                    targetPath = new FileStructureDocumenPath
                    {
                        DirectoryGuid = firstDirectory.Id,
                        FileStructureGuid = targetStructure.Id,
                        Id = Guid.NewGuid(),
                        DocumentGuid = workflowOperation.DocumentId,
                        WorkflowId = workflowOperation.WorkflowId,
                        IsProtectedPath = false,
                        WorkflowState = DocumentWorkflowStateType.InReview
                    };
                }

                var tracker = new DocumentWorkflowTracker
                {
                    ActionName = DocumentWorkflowStateType.Forwarded,
                    CreateDateTime = DateTime.Now,
                    DocumentId = targetPath.DocumentGuid,
                    TargetUserId = workflowOperation.TargetUserId,
                    UserId = workflowOperation.UserId
                };

                documentWorkflowTrackerService.Save(tracker);
                fileStructureDocumentPathService.Save(targetPath);

                if (accessProvider != null)
                    accessProvider.SetUserAccess(workflowOperation.TargetUserId, workflowOperation.DocumentId, targetPath.Id, targetStructure.Id, configuration);
            }
            else
            {
                SaveWorkflowOrganizationUnitAssignment(workflowOperation, configuration.StateProviderName);
                TrackChanges(workflowOperation);
                if (accessProvider != null && workflowOperation.WorkflowOrganizationId.HasValue)
                    accessProvider.SetOrganizationUnitAcess(workflowOperation.WorkflowOrganizationId.Value, workflowOperation.DocumentId, configuration); 
            }

            var path = fileStructureDocumentPathService.Get(workflowOperation.DocumentPath);
            path.WorkflowState = DocumentWorkflowStateType.Completed;
            fileStructureDocumentPathService.Save(path);
        }

        /// <summary>
        /// Sends a copy to the target user
        /// </summary>
        /// <param name="workflowOperation"></param>
        public void ForwardCopyTo(WorkflowOperation workflowOperation)
        {
            var configuration = documentWorkflowConfigurationService.Get(workflowOperation.WorkflowId);
            var accessProvider = unityContainer.Resolve<IDocumentWorkflowAccessProvider>(configuration.AccessProviderName);

            if (workflowOperation.OperationType == WorkflowOperationType.WorkflowOrganizationUnit)
            {
                SaveWorkflowOrganizationUnitAssignment(workflowOperation, configuration.StateProviderName);
                TrackChanges(workflowOperation);

                if (accessProvider != null && workflowOperation.WorkflowOrganizationId.HasValue) 
                    accessProvider.SetOrganizationUnitAcess(workflowOperation.WorkflowOrganizationId.Value, workflowOperation.DocumentId, configuration); 
            }
            else
            {

                // Add path to forwarded user
                var workflow = documentWorkflowUserService.Get(workflowOperation.TargetUserId);
                if (workflow == null)
                {
                    throw new DocumentWorkflowException("workflow is null");
                }

                var targetStructure = fileStructureService.GetByInstanceDataGuid(workflow.Guid);
                if (targetStructure == null)
                {
                    throw new DocumentWorkflowException("targetStructure is null");
                }

                // TODO: Check for null
                var existingStructures = fileStructureDocumentPathService.GetByDocumentId(workflowOperation.DocumentId)
                                                                         .ToList();
                if (existingStructures == null)
                {
                    throw new DocumentWorkflowException("existingStructures is null");
                }

                var targetPath = existingStructures.FirstOrDefault(x => x.FileStructureGuid == targetStructure.Id);

                if (targetPath != null)
                {
                    targetPath.WorkflowState = DocumentWorkflowStateType.InReview;

                }
                else
                {
                    var firstDirectory = FindWorkflowDirectory(targetStructure, workflowOperation.WorkflowId);

                    if (firstDirectory == null)
                    {
                        throw new DocumentWorkflowException("existingStructures is null");
                    }

                    targetPath = new FileStructureDocumenPath
                    {
                        DirectoryGuid = firstDirectory.Id,
                        WorkflowId = firstDirectory.WorkflowId,
                        FileStructureGuid = targetStructure.Id,
                        Id = Guid.NewGuid(),
                        DocumentGuid = workflowOperation.DocumentId,
                        IsProtectedPath = false,
                        WorkflowState = DocumentWorkflowStateType.InReview
                    };
                }

                var tracker = new DocumentWorkflowTracker
                {
                    ActionName = DocumentWorkflowStateType.ForwardedCopy,
                    CreateDateTime = DateTime.Now,
                    DocumentId = targetPath.DocumentGuid,
                    TargetUserId = workflowOperation.TargetUserId,
                    UserId = workflowOperation.UserId
                };

                documentWorkflowTrackerService.Save(tracker);
                fileStructureDocumentPathService.Save(targetPath);

                if (accessProvider != null)
                    accessProvider.SetUserAccess(workflowOperation.TargetUserId, workflowOperation.DocumentId, targetPath.Id, targetStructure.Id, configuration);
            }
        }

        private void SaveWorkflowOrganizationUnitAssignment(WorkflowOperation workflowOperation, string stateProvider)
        {
            var documentWorkflowStateProvider = unityContainer.Resolve<IDocumentWorkflowStateProvider>(stateProvider);

            var documentWorkflowOrganzitionUnitAssignment = new DocumentWorkflowOrganizationUnitAssignment
            {
                DocumentId = workflowOperation.DocumentId,
                WorkflowOrganizationUnitId = (Guid)workflowOperation.WorkflowOrganizationId, 
                WorkflowId = workflowOperation.WorkflowId
            };
            documentWorkflowOrganizationUnitAssignmentService.Save(documentWorkflowOrganzitionUnitAssignment);

            var state = documentWorkflowStateProvider.ResolveDocumentWorkflowState(workflowOperation.DocumentId, workflowOperation.WorkflowId);
            documentWorkflowAssignmentService.Save(new DocumentWorkflowAssignment
            {
                DocumentId = workflowOperation.DocumentId,
                StateId = state.Guid,
                WorkflowId = workflowOperation.WorkflowId
            });
        }

        /// <summary>
        /// Sets the state to complete
        /// </summary>
        /// <param name="workflowOperation"></param>
        public void Complete(WorkflowOperation workflowOperation)
        {
            var path = fileStructureDocumentPathService.Get(workflowOperation.DocumentPath);
            path.WorkflowState = DocumentWorkflowStateType.Completed;

            var tracker = new DocumentWorkflowTracker
            {
                ActionName = DocumentWorkflowStateType.Completed,
                CreateDateTime = DateTime.Now,
                DocumentId = workflowOperation.DocumentId,
                UserId = workflowOperation.UserId
            };

            documentWorkflowTrackerService.Save(tracker);
            fileStructureDocumentPathService.Save(path);
        }

        /// <summary>
        /// Checks the document out of a workflow organization unit 
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns>Document path id</returns>
        public Guid DocumentCheckout(WorkflowOperation workflowOperation)
        {

            //Check if the document is still inside 
            if (documentWorkflowOrganizationUnitAssignmentService.GetByIds(workflowOperation.DocumentId, (Guid)workflowOperation.WorkflowOrganizationId) != null)
            {
                documentWorkflowOrganizationUnitAssignmentService.DeleteByIds(workflowOperation.DocumentId, (Guid)workflowOperation.WorkflowOrganizationId);

                // Add path to forwarded user
                var workflow = documentWorkflowUserService.Get(workflowOperation.TargetUserId);
                if (workflow == null)
                {
                    throw new DocumentWorkflowException("workflow is null");
                }
                var targetStructure = fileStructureService.GetByInstanceDataGuid(workflow.Guid);
                if (targetStructure == null)
                {
                    throw new DocumentWorkflowException("targetStructure is null");
                }

                var targetPath = new FileStructureDocumenPath
                {
                    DirectoryGuid = (Guid)workflowOperation.DirectoryId,
                    WorkflowId = workflowOperation.WorkflowId,
                    FileStructureGuid = targetStructure.Id,
                    Id = Guid.NewGuid(),
                    DocumentGuid = workflowOperation.DocumentId,
                    IsProtectedPath = false,
                    WorkflowState = DocumentWorkflowStateType.InReview
                };

                var tracker = new DocumentWorkflowTracker
                {
                    ActionName = DocumentWorkflowStateType.ForwardedCopy,
                    CreateDateTime = DateTime.Now,
                    DocumentId = targetPath.DocumentGuid,
                    TargetUserId = workflowOperation.TargetUserId,
                    UserId = workflowOperation.UserId
                };

                documentWorkflowTrackerService.Save(tracker);
                fileStructureDocumentPathService.Save(targetPath);

                return targetPath.Id;
            }
            throw new CoreException("S-0000009", "9ae836fb-40e8-43d3-9d57-85c17647684a", ExceptionType.Expected);
        }
    }
}
