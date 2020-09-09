using CommonServiceLocator;
using Simplic.DataStack;
using Simplic.FileStructure.Workflow;
using Simplic.Framework.DBUI;
using Simplic.Icon;
using Simplic.Localization;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Document overview control view model
    /// </summary>
    public class DocumentPathOverViewViewModel : ViewModelBase
    {
        private Guid documentId;
        private ObservableCollection<DocumentPathViewModel> paths;

        private readonly IFileStructureDocumentPathService documentPathService;
        private readonly IFileStructureService fileStructureService;
        private readonly IDirectoryTypeService directoryTypeService;
        private readonly IIconService iconService;
        private readonly ILocalizationService localizationService;
        private readonly IStackService stackService;
        private readonly IDocumentWorkflowAssignmentService documentWorkflowAssignmentService;

        private ICommand addDocumentPathCommand;
        private ICommand changeDocumentPathCommand;
        private ICommand removeDocumentPathCommand;

        private IList<DocumentPathViewModel> removedPaths;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="documentId">Document id</param>
        public DocumentPathOverViewViewModel(Guid documentId)
        {
            this.documentId = documentId;

            documentPathService = ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();
            fileStructureService = ServiceLocator.Current.GetInstance<IFileStructureService>();
            directoryTypeService = ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            iconService = ServiceLocator.Current.GetInstance<IIconService>();
            localizationService = ServiceLocator.Current.GetInstance<ILocalizationService>();
            stackService = ServiceLocator.Current.GetInstance<IStackService>();
            documentWorkflowAssignmentService = ServiceLocator.Current.GetInstance<IDocumentWorkflowAssignmentService>();

            removedPaths = new List<DocumentPathViewModel>();

            paths = new ObservableCollection<DocumentPathViewModel>();
            foreach (var path in documentPathService.GetByDocumentId(documentId))
            {
                var pathVM = new DocumentPathViewModel(path, fileStructureService, directoryTypeService, iconService, stackService)
                {
                    Parent = this
                };

                paths.Add(pathVM);
            }

            // Add new document path
            addDocumentPathCommand = new RelayCommand((p) =>
            {
                var newDocumentPath = SelectPath();

                if (newDocumentPath != null)
                {
                    if (CheckPathExists(newDocumentPath.Id, newDocumentPath.DirectoryGuid, newDocumentPath.FileStructureGuid))
                    {
                        MessageBox.Show(localizationService.Translate("filestructure_path_exists"), localizationService.Translate("filestructure_path_exists_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var newDocumentPathVM = new DocumentPathViewModel(newDocumentPath, fileStructureService, directoryTypeService, iconService, stackService)
                    {
                        Parent = this
                    };

                    Paths.Add(newDocumentPathVM);
                }
            });

            // Change document path
            changeDocumentPathCommand = new RelayCommand((p) =>
            {
                var selectedPath = p as DocumentPathViewModel;

                if (selectedPath.IsProtectedPath)
                {
                    MessageBox.Show(localizationService.Translate("filestructure_path_protected"), localizationService.Translate("filestructure_path_protected_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var fileStructure = fileStructureService.Get(selectedPath.Model.FileStructureGuid);


                var selectPathWindow = new FileStructureWindow();
                selectPathWindow.Initialize(fileStructure);
                selectPathWindow.IsInSelectMode = true;

                selectPathWindow.Loaded += (s, e) =>
                {
                    selectPathWindow.ViewModel.SelectedDirectory = selectPathWindow.ViewModel.RawDirectories.FirstOrDefault(x => x.Model.Id == selectedPath.Model.DirectoryGuid);
                };

                selectPathWindow.ShowDialog();

                if (selectPathWindow.SelectedDirectory != null)
                {
                    if (CheckPathExists(selectedPath.Model.Id, selectPathWindow.SelectedDirectory.Id, fileStructure.Id))
                    {
                        MessageBox.Show(localizationService.Translate("filestructure_path_exists"), localizationService.Translate("filestructure_path_exists_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    selectedPath.Model.DirectoryGuid = selectPathWindow.SelectedDirectory.Id;
                    selectedPath.RefreshPath();
                }
            });

            // Remove document path
            removeDocumentPathCommand = new RelayCommand((p) =>
            {
                var selectedPath = p as DocumentPathViewModel;

                if (selectedPath.IsProtectedPath)
                {
                    MessageBox.Show(localizationService.Translate("filestructure_path_protected"), localizationService.Translate("filestructure_path_protected_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var messageBoxResult = MessageBox.Show(localizationService.Translate("filestructure_remove_path"), localizationService.Translate("filestructure_remove_path_title"), MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    removedPaths.Add(selectedPath);
                    paths.Remove(selectedPath);
                }
            });
        }

        /// <summary>
        /// Check whether a path is already exists
        /// </summary>
        /// <param name="pathId">Unique path id</param>
        /// <param name="directoryId">Directory id</param>
        /// <param name="fileStructureId">File structure id</param>
        /// <returns>True if the path is already existing</returns>
        private bool CheckPathExists(Guid pathId, Guid directoryId, Guid fileStructureId)
        {
            return paths.Any(x => x.Model.Id != pathId && x.Model.DirectoryGuid == directoryId && x.Model.FileStructureGuid == fileStructureId);
        }

        /// <summary>
        /// Select document path
        /// </summary>
        /// <returns></returns>
        private FileStructureDocumenPath SelectPath()
        {
            // Show stack selection
            var selectStackItemBox = ItemBoxManager.GetItemBoxFromDB("IB_FileStructure_Stack");
            selectStackItemBox.ShowDialog();

            // Select instance-data
            if (selectStackItemBox.SelectedItem != null)
            {
                var selectDataItemBox = ItemBoxManager.GetItemBoxFromDB(selectStackItemBox.GetSelectedItemCell("StackDataItemBox").ToString());
                selectDataItemBox.ShowDialog();

                if (selectDataItemBox.SelectedItem != null)
                {
                    var fileStructure = fileStructureService.GetByInstanceDataGuid((Guid)selectDataItemBox.GetSelectedItemCell("Guid"));
                    if (fileStructure == null)
                    {
                        MessageBox.Show(localizationService.Translate("filestructure_path_no_selection"), localizationService.Translate("filestructure_path_no_selection_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var selectPathWindow = new FileStructureWindow();
                        selectPathWindow.Initialize(fileStructure);
                        selectPathWindow.IsInSelectMode = true;

                        selectPathWindow.ShowDialog();

                        if (selectPathWindow.SelectedDirectory != null)
                        {
                            var newDocumentPath = new FileStructureDocumenPath
                            {
                                Id = Guid.NewGuid(),
                                DirectoryGuid = selectPathWindow.SelectedDirectory.Id,
                                DocumentGuid = documentId,
                                FileStructureGuid = fileStructure.Id,
                                WorkflowId = selectPathWindow.SelectedDirectory.WorkflowId
                               
                            };
                            
                            return newDocumentPath;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            // Remove path
            foreach (var path in removedPaths)
                documentPathService.Delete(path.Model);

            removedPaths.Clear();

            foreach (var path in paths)
            {
                var fileStructure = fileStructureService.Get(path.Model.FileStructureGuid);
                path.Model.WorkflowId = fileStructure.Directories.First().WorkflowId;
                documentPathService.Save(path.Model);
            }

            bool isWorkflowFolder = false;
            Guid worklfowId = Guid.Empty;
            var file = fileStructureService.Get(paths.Last().Model.FileStructureGuid);

            foreach (var directory in file.Directories)
            {
                if (directory.WorkflowId.HasValue)
                {
                    isWorkflowFolder = true;
                    worklfowId = (Guid)directory.WorkflowId;
                }
            }

            if (isWorkflowFolder)
            {
                var documentWorkflowAssignment = new DocumentWorkflowAssignment
                {
                    DocumentId = documentId,
                    WorkflowId = worklfowId,
                };
                documentWorkflowAssignmentService.Save(documentWorkflowAssignment);
            }

            IsDirty = false;
        }

        /// <summary>
        /// Gets or sets the list of document paths
        /// </summary>
        public ObservableCollection<DocumentPathViewModel> Paths
        {
            get
            {
                return paths;
            }

            set
            {
                paths = value;
            }
        }

        /// <summary>
        /// Gets or sets the add document path command
        /// </summary>
        public ICommand AddDocumentPathCommand
        {
            get
            {
                return addDocumentPathCommand;
            }

            set
            {
                addDocumentPathCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the change document path command
        /// </summary>
        public ICommand ChangeDocumentPathCommand
        {
            get
            {
                return changeDocumentPathCommand;
            }

            set
            {
                changeDocumentPathCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the remove document path command
        /// </summary>
        public ICommand RemoveDocumentPathCommand
        {
            get
            {
                return removeDocumentPathCommand;
            }

            set
            {
                removeDocumentPathCommand = value;
            }
        }
    }
}
