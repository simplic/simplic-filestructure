using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowStateProviderService : IDocumentWorkflowStateProviderService
    {
        private readonly IDocumentWorkflowAssignmentService documentWorkflowAssignmentService;
        private readonly IDocumentWorkflowStateService documentWorkflowStateService;
        private readonly IFileStrucutreDocumentPathService fileStrucutreDocumentPathService;

        public DocumentWorkflowStateProviderService(IDocumentWorkflowAssignmentService documentWorkflowAssignmentService,
            IDocumentWorkflowStateService documentWorkflowStateService)
        {
            this.documentWorkflowStateService = documentWorkflowStateService;
            this.documentWorkflowAssignmentService = documentWorkflowAssignmentService;
        }

        public DocumentWorkflowState GetNewDocumentWorkflowState(Guid documentId, Guid workflowId)
        {
            var state = new DocumentWorkflowState();
            documentWorkflowAssignmentService.AlreadyExists(documentId, workflowId);


            return state;
        }
    }

}
