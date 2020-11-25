using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IWorkflowOperationService"/>
    /// </summary>
    public class WorkflowOperationService : IWorkflowOperationService
    {
        private readonly IFileStructureService fileStructureService;
        private readonly IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService;
        private readonly IDocumentWorkflowUserService documentWorkflowUserService;
        private readonly IFileStructureDocumentPathService fileStructureDocumentPathService;
        private readonly IDocumentWorkflowTrackerService documentWorkflowTrackerService;
        private readonly IDocumentWorkflowOrganizationUnitAssignmentService documentWorkflowOrganizationUnitAssignmentService;


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
                                        IDocumentWorkflowOrganizationUnitAssignmentService documentWorkflowOrganizationUnitAssignmentService)
        {
            this.fileStructureService = fileStructureService;
            this.documentWorkflowAppSettingsService = documentWorkflowAppSettingsService;
            this.documentWorkflowUserService = documentWorkflowUserService;
            this.fileStructureDocumentPathService = fileStructureDocumentPathService;
            this.documentWorkflowTrackerService = documentWorkflowTrackerService;
            this.documentWorkflowOrganizationUnitAssignmentService = documentWorkflowOrganizationUnitAssignmentService;
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
        
        /// <summary>
        /// Sends the document to the target user id user
        /// </summary>
        /// <param name="workflowOperation"></param>
        public void ForwardTo(WorkflowOperation workflowOperation)
        {
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
            }
            else
            {
                SaveWorkflowOrganizationUnitAssignment(workflowOperation);
            }

            //immer
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
            if (workflowOperation.OperationType == WorkflowOperationType.WorkflowOrganizationUnit)
            {
                SaveWorkflowOrganizationUnitAssignment(workflowOperation);
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
            }
        }

        private void SaveWorkflowOrganizationUnitAssignment(WorkflowOperation workflowOperation)
        {
            var documentWorkflowOrganzitionUnitAssignment = new DocumentWorkflowOrganizationUnitAssignment
            {
                DocumentId = workflowOperation.DocumentId,
                WorkflowOrganizationUnitId = (Guid)workflowOperation.WorkflowOrganzisationId,
            };
            documentWorkflowOrganizationUnitAssignmentService.Save(documentWorkflowOrganzitionUnitAssignment);
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
        public void DocumentCheckOut(WorkflowOperation workflowOperation)
        {
            documentWorkflowOrganizationUnitAssignmentService.DeleteByIds(workflowOperation.DocumentId, (Guid)workflowOperation.WorkflowOrganzisationId);
            ForwardCopyTo(workflowOperation);
        }
        //Checkout dokument auschecken für die wou und in den user packen 
    }
}
