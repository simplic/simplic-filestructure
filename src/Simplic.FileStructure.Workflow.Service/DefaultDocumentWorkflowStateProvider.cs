using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DefaultDocumentWorkflowStateProvider : IDocumentWorkflowStateProvider
    {
        private readonly IDocumentWorkflowAssignmentService documentWorkflowAssignmentService;
        private readonly IDocumentWorkflowStateService documentWorkflowStateService;
        private readonly IDocumentWorkflowStateProviderRepository documentWorkflowStateProviderRepository;

        public DefaultDocumentWorkflowStateProvider(IDocumentWorkflowAssignmentService documentWorkflowAssignmentService,
            IDocumentWorkflowStateService documentWorkflowStateService, IDocumentWorkflowStateProviderRepository documentWorkflowStateProviderRepository)
        {
            this.documentWorkflowStateService = documentWorkflowStateService;
            this.documentWorkflowAssignmentService = documentWorkflowAssignmentService;
            this.documentWorkflowStateProviderRepository = documentWorkflowStateProviderRepository;
        }

        public DocumentWorkflowState ResolveDocumentWorkflowState(Guid documentId, Guid workflowId)
        {
            var resultState = new DocumentWorkflowState
            {
                InternalName = "StillInWorkflow",
                Name = "InProgress"
            };
            if (documentWorkflowAssignmentService.AlreadyExists(documentId, workflowId))
            {
                if (documentWorkflowStateProviderRepository.IsDocumentInWorkflowCompleted(documentId, workflowId))
                {
                    resultState.Name = "Completed";
                    resultState.InternalName = "CompletedWorkflow";
                }
            }
            return resultState;
        }
    }

}
