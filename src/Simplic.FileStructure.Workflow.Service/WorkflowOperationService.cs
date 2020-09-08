using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class WorkflowOperationService : IWorkflowOperationService
    {
        private readonly IFileStructureService fileStructureService;
        private readonly IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService;
        private readonly IDocumentWorkflowUserService documentWorkflowUserService;
        private readonly IFileStructureDocumentPathService fileStructureDocumentPathService;
        private readonly IDocumentWorkflowTrackerService documentWorkflowTrackerService;

        public WorkflowOperationService(IFileStructureService fileStructureService,
                                        IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService, 
                                        IDocumentWorkflowUserService documentWorkflowUserService,
                                        IFileStructureDocumentPathService fileStructureDocumentPathService,
                                        IDocumentWorkflowTrackerService documentWorkflowTrackerService)
        {
            this.fileStructureService = fileStructureService;
            this.documentWorkflowAppSettingsService = documentWorkflowAppSettingsService;
            this.documentWorkflowUserService = documentWorkflowUserService;
            this.fileStructureDocumentPathService = fileStructureDocumentPathService;
            this.documentWorkflowTrackerService = documentWorkflowTrackerService;
        }

        public void ForwardTo(WorkflowOperation workflowOperation)
        {


            // Add path to forwarded user
            var workflow = documentWorkflowUserService.Get(workflowOperation.InternalWorkflowName, workflowOperation.TargetUserId);

            // TODO: Check for null

            var targetStructure = fileStructureService.GetByInstanceDataGuid(workflow.Guid);

            // TODO: Check for null
            var existingStructures = fileStructureDocumentPathService.GetByDocumentId(workflowOperation.DocumentId)
                                                                     .ToList();

            var targetPath = existingStructures.FirstOrDefault(x => x.FileStructureGuid == targetStructure.Id);

            if (targetPath != null)
            {
                targetPath.WorkflowState = DocumentWorkflowStateType.InReview;
            }
            else
            {
                var firstDirectory = targetStructure.Directories.FirstOrDefault();

                // TODO: Check for null

                targetPath = new FileStructureDocumenPath 
                {
                    DirectoryGuid = firstDirectory.Id,
                    FileStructureGuid = targetStructure.Id,
                    Id = Guid.NewGuid(),
                    DocumentGuid = workflowOperation.DocumentId,
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
            
            var path = fileStructureDocumentPathService.Get(workflowOperation.DocumentPath);
            path.WorkflowState = DocumentWorkflowStateType.Completed;

            fileStructureDocumentPathService.Save(path);
        }

        public void ForwardCopyTo(WorkflowOperation workflowOperation)
        {
            // Add path to forwarded user
            var workflow = documentWorkflowUserService.Get(workflowOperation.InternalWorkflowName, workflowOperation.TargetUserId);
            if (workflow == null)
            {
                throw new Exception("workflow is null");
            }
            // TODO: Check for null

            var targetStructure = fileStructureService.GetByInstanceDataGuid(workflow.Guid);
            if (targetStructure == null)
            {
                throw new Exception("targetStructure is null");
            }

            // TODO: Check for null
            var existingStructures = fileStructureDocumentPathService.GetByDocumentId(workflowOperation.DocumentId)
                                                                     .ToList();
            if (existingStructures == null)
            {
                throw new Exception("existingStructures is null");
            }

            var targetPath = existingStructures.FirstOrDefault(x => x.FileStructureGuid == targetStructure.Id);

            if (targetPath != null)
            {
                targetPath.WorkflowState = DocumentWorkflowStateType.InReview;
            }
            else
            {
                var firstDirectory = targetStructure.Directories.FirstOrDefault();

                // TODO: Check for null

                targetPath = new FileStructureDocumenPath
                {
                    DirectoryGuid = firstDirectory.Id,
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
    }
}
