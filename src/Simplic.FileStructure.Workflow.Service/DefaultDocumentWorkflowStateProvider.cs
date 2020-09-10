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
		private readonly IDocumentWorkflowStateService documentWorkflowStateService;
		private readonly IDocumentWorkflowStateProviderRepository documentWorkflowStateProviderRepository;

		public DefaultDocumentWorkflowStateProvider(IDocumentWorkflowStateService documentWorkflowStateService, IDocumentWorkflowStateProviderRepository documentWorkflowStateProviderRepository)
		{
			this.documentWorkflowStateService = documentWorkflowStateService;
			this.documentWorkflowStateProviderRepository = documentWorkflowStateProviderRepository;
		}

		private readonly DocumentWorkflowState inProgressState = new DocumentWorkflowState
		{
			Guid = Guid.Parse("f7811c53-84e5-4bcb-8ada-93546176dd42"),
			InternalName = "InProgress",
			Name = "InProgress",
			StateType = DocumentWorkflowStateType.InReview
		};

		private readonly DocumentWorkflowState completedState = new DocumentWorkflowState
		{
			Guid = Guid.Parse("d17d9972-07bf-4ccd-99f1-60f2190a3a76"),
			InternalName = "Completed",
			Name = "Completed",
			StateType = DocumentWorkflowStateType.Completed
		};

		public DocumentWorkflowState ResolveDocumentWorkflowState(Guid documentId, Guid workflowId)
		{
			// TODO: Just save if not existing
			documentWorkflowStateService.Save(inProgressState);
			documentWorkflowStateService.Save(completedState);

			if (documentWorkflowStateProviderRepository.IsDocumentInWorkflowCompleted(documentId, workflowId))
				return completedState;

			return inProgressState;
		}

		/// <summary>
		/// Gets the state provider name
		/// </summary>
		public string Name => "default";
	}
}
