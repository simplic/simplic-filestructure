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

        public DocumentWorkflowStateProviderService(IDocumentWorkflowAssignmentService documentWorkflowAssignmentService,
            IDocumentWorkflowStateService documentWorkflowStateService)
        {
            this.documentWorkflowStateService = documentWorkflowStateService;
            this.documentWorkflowAssignmentService = documentWorkflowAssignmentService;
        }

        public DocumentWorkflowStateType GetNewDocumentWorkflowState(Guid documentId, Guid workflowId)
        {
            throw new NotImplementedException();
        }
    }

}
