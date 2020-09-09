using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowStateProviderService : IDocumentWorkflowStateProviderService
    {
        private readonly IDocumentWorkflowAssignmentService documentWorkflowAssignmentService;
        private readonly IDocumentWorkflowStateService documentWorkflowStateService;
        private readonly IFileStructureDocumentPathService fileStructureDocumentPathService;

        public DocumentWorkflowStateProviderService(IDocumentWorkflowAssignmentService documentWorkflowAssignmentService,
            IDocumentWorkflowStateService documentWorkflowStateService,
            IFileStructureDocumentPathService fileStructureDocumentPathService)
        {
            this.documentWorkflowStateService = documentWorkflowStateService;
            this.documentWorkflowAssignmentService = documentWorkflowAssignmentService;
            this.fileStructureDocumentPathService = fileStructureDocumentPathService;
        }

        public DocumentWorkflowState GetNewDocumentWorkflowState(Guid documentId, Guid workflowId)
        {
            var resultState = new DocumentWorkflowState();
            if (documentWorkflowAssignmentService.AlreadyExists(documentId, workflowId))
            {
                var states = new List<DocumentWorkflowStateType>();
                var documents = fileStructureDocumentPathService.GetByDocumentId(documentId);

                foreach (var document in documents)
                    if (document.WorkflowId.Equals(workflowId))
                        states.Add(document.WorkflowState);

                if (states.Any())
                {
                    var tmp = DocumentWorkflowStateType.Completed;
                    foreach (var state in states)
                    {
                        if (state < tmp)
                        {
                            tmp = state;
                        }
                    }
                    //Lowest type is now tmp
                    resultState.DocumentWorkflowStateType = tmp;
                }


            }

            return resultState;
        }
    }

}
