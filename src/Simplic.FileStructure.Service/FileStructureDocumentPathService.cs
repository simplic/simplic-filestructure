using Simplic.FileStructure.Workflow;
using Simplic.Session;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Simplic.FileStructure.Service
{
	/// <summary>
	/// File structure service
	/// </summary>
	public class FileStructureDocumentPathService : IFileStructureDocumentPathService
	{
		private readonly IFileStructureDocumentPathRepository repository;
		private readonly IFileStructureDocumentPathTrackingRepository fileStructureDocumentPathTrackingRepository;
		private readonly IFileStructureService structureService;
		private readonly ISessionService sessionService;
		private readonly IDocumentWorkflowAssignmentService documentWorkflowAssignmentService;
		private readonly IDocumentWorkflowConfigurationService documentWorkflowConfigurationService;
		private readonly IUnityContainer unityContainer;

		/// <summary>
		/// Initialize service
		/// </summary>
		/// <param name="repository">Repository instance</param>
		/// <param name="structureService">File structure service</param>
		public FileStructureDocumentPathService(IFileStructureDocumentPathRepository repository
			, IFileStructureService structureService
			, IFileStructureDocumentPathTrackingRepository fileStructureDocumentPathTrackingRepository
			, ISessionService sessionService
			, IDocumentWorkflowAssignmentService documentWorkflowAssignmentService
			, IDocumentWorkflowConfigurationService documentWorkflowConfigurationService
			, IUnityContainer unityContainer)
		{
			this.repository = repository;
			this.structureService = structureService;
			this.fileStructureDocumentPathTrackingRepository = fileStructureDocumentPathTrackingRepository;
			this.sessionService = sessionService;
			this.documentWorkflowAssignmentService = documentWorkflowAssignmentService;
			this.documentWorkflowConfigurationService = documentWorkflowConfigurationService;
			this.unityContainer = unityContainer;
		}

		/// <summary>
		/// Delete a path instance
		/// </summary>
		/// <param name="id">Id to delete</param>
		/// <returns>True if successull</returns>
		public bool Delete(Guid id)
		{
			return repository.Delete(id);
		}

		/// <summary>
		/// Delete path by object id
		/// </summary>
		/// <param name="obj">Object to delete</param>
		/// <returns>True if successfull</returns>
		public bool Delete(FileStructureDocumenPath obj)
		{
			return repository.Delete(obj);
		}

		/// <summary>
		/// Get file path instance
		/// </summary>
		/// <param name="id">Structure id</param>
		/// <returns>Structure instance of found</returns>
		public FileStructureDocumenPath Get(Guid id)
		{
			return repository.Get(id);
		}

		/// <summary>
		/// Gets all file structure instance
		/// </summary>
		/// <returns>Structure instance</returns>
		public IEnumerable<FileStructureDocumenPath> GetAll()
		{
			return repository.GetAll();
		}

		/// <summary>
		/// Save file structure to database
		/// </summary>
		/// <param name="obj">Object to save</param>
		/// <returns>True if successfull</returns>
		public bool Save(FileStructureDocumenPath obj)
		{
			var fileStructure = structureService.Get(obj.FileStructureGuid);
			obj.Path = "";

			if (fileStructure != null)
			{
				var currentItem = fileStructure.Directories.FirstOrDefault(x => x.Id == obj.DirectoryGuid);
				while (currentItem != null)
				{
					obj.Path = obj.Path.Insert(0, $"/{currentItem.Name}");

					if (currentItem.Parent != null)
					{
						currentItem = currentItem.Parent;
					}
					else
					{
						currentItem = null;
						break;
					}
				}
			}

			var result = repository.Save(obj);

			// Path has changed
			if (obj.PreviousPath != obj.Path || obj.PreviousWorkflowState != obj.WorkflowState)
			{
				try
				{
					fileStructureDocumentPathTrackingRepository.Save(new FileStructureDocumenPathTracking
					{
						Id = Guid.NewGuid(),
						DirectoryGuid = obj.DirectoryGuid,
						DocumentGuid = obj.DocumentGuid,
						WorkflowState = obj.WorkflowState,
						FileStructureGuid = obj.FileStructureGuid,
						FileStructureHash = obj.FileStructureHash,
						IsProtectedPath = obj.IsProtectedPath,
						Path = obj.Path ?? "",
						StorageHash = obj.StorageHash,
						UserId = sessionService.CurrentSession.UserId,
						PreviousPath = obj.PreviousPath ?? ""
					});

					if (obj.WorkflowId.HasValue)
					{
						var workflowId = obj.WorkflowId.Value;

						var workflow = documentWorkflowConfigurationService.Get(workflowId);
						if (workflow == null) throw new Exception($"Could not find workflow: {workflowId}");

						var stateProvider = unityContainer.Resolve<IDocumentWorkflowStateProvider>(workflow.StateProviderName);

						if (!documentWorkflowAssignmentService.Exists(obj.DocumentGuid, workflowId))
						{
							var state = stateProvider.ResolveDocumentWorkflowState(obj.DocumentGuid, workflowId);
							if (state == null) throw new Exception($"Could not resolve initial state for document: {obj.DocumentGuid}");

							var assignment = new DocumentWorkflowAssignment
							{
								DocumentId = obj.DocumentGuid,
								WorkflowId = workflowId,
								StateId = state.Guid
							};

							documentWorkflowAssignmentService.Save(assignment);
						}
						else
						{
							var state = stateProvider.ResolveDocumentWorkflowState(obj.DocumentGuid, workflowId);
							if (state == null) throw new Exception($"Could not resolve initial state for document: {obj.DocumentGuid}");

							documentWorkflowAssignmentService.SetState(obj.DirectoryGuid, workflowId, state.Guid);
						}
					}
				}
				catch (Exception ex)
				{
					Log.LogManagerInstance.Instance.Error($"Could not write file structure path tracking: Document id: {obj.DocumentGuid}", ex);
				}
			}

			return result;
		}

		/// <summary>
		/// Get all by document id
		/// </summary>
		/// <param name="documentId">Document id</param>
		/// <returns>Enumerable of paths</returns>
		public IEnumerable<FileStructureDocumenPath> GetByDocumentId(Guid documentId)
		{
			return repository.GetByDocumentId(documentId);
		}


		/// <summary>
		/// Gets all by directory id 
		/// </summary>
		/// <param name="directoryId"></param>
		/// <returns></returns>
		public IEnumerable<FileStructureDocumenPath> GetByDirectoryId(Guid directoryId)
		{
			return repository.GetByDirectoryId(directoryId);
		}

		/// <summary>
		/// Goes through a list of directories and checks, whether one of the directory contains a protected document
		/// </summary>
		/// <param name="directoryIds">List of directory ids</param>
		/// <returns>True if any document inside of the given directories is protected.</returns>
		public bool IsProtected(IList<Guid> directoryIds)
		{
			return repository.IsProtected(directoryIds);
		}
	}
}
