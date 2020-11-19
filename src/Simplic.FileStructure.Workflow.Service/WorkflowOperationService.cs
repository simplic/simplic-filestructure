using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simplic.FileStructure.Workflow.Service
{
    public class WorkflowOperationService : IWorkflowOperationService
    {
        private readonly IFileStructureService fileStructureService;
        private readonly IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService;
        private readonly IDocumentWorkflowUserService documentWorkflowUserService;
        private readonly IFileStructureDocumentPathService fileStructureDocumentPathService;
        private readonly IDocumentWorkflowTrackerService documentWorkflowTrackerService;
        private readonly IDocumentWorkflowOrganizationUnitAssignmentService documentWorkflowOrganizationUnitAssignmentService;


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
        //
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

        public void ForwardCopyTo(WorkflowOperation workflowOperation)
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
            if (workflowOperation.OperationType == WorkflowOperationType.WorkflowOrganizationUnit)
            {
                SaveWorkflowOrganizationUnitAssignment(workflowOperation);
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
        //Checkout dokument auschecken für die wou und in den user packen 
    }
}
