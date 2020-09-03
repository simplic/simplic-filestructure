using Simplic.DataStack;
using Simplic.FileStructure.Model;
using Simplic.FileStructure.Workflow;
using Simplic.Framework.DBUI;
using Simplic.Framework.UI;
using Simplic.Icon;
using Simplic.Localization;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// File structure editor viewmodel
    /// </summary>
    public class FileStructureViewModel : ExtendableViewModel, Studio.UI.IWindowViewModel<FileStructure>, IDirectoryBaseViewModel
    {
        private ObservableCollection<DirectoryViewModel> directories;
        private ObservableCollection<RadMenuItem> directoryCategoryMenuItems;
        private IList<DirectoryViewModel> rawDirectories;

        private Boolean expanderIsExpanded;

        private DirectoryViewModel selectedDirectory;
        private FileStructure model;
        private RadTreeView directoryTreeView;
        private Expander expander;
        private ObservableCollection<FrameworkElement> visualPathElements;
        private string rootPath;
        private string assignedWorkflow;

        private ICommand removeDirectoryCommand;
        private ICommand archiveFromClipboard;
        private ICommand archiveFromScanner;
        private ICommand editMetaDataCommand;
        private ICommand renameDirectoryCommand;
        private ICommand assignWorkflowCommand;

        private readonly ILocalizationService localizationService;
        private readonly IIconService iconService;
        private readonly IDirectoryTypeService directoryTypeService;
        private readonly IStackService stackService;
        private readonly IFileStructureService fielStructureService;
        private readonly IDocumentWorkflowContextService documentWorkflowContextService;

        private readonly IDirectoryFieldService directoryFieldService;
        private readonly IDirectoryClassificationFieldService directoryTypeFieldService;

        /// <summary>
        /// Create view model
        /// </summary>
        public FileStructureViewModel(RadTreeView directoryTreeView, Expander expander)
        {
            this.directoryTreeView = directoryTreeView;
            this.expander = expander;

            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
            directoryTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            iconService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IIconService>();
            stackService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IStackService>();
            fielStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            documentWorkflowContextService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentWorkflowContextService>();


            directoryFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryFieldService>();
            directoryTypeFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationFieldService>();

            rootPath = "";
            VisualPathElements = new ObservableCollection<FrameworkElement>();

            directoryCategoryMenuItems = new ObservableCollection<RadMenuItem>();

            foreach (var category in directoryTypeService.GetAll().OrderBy(d => d.Category).GroupBy(d => d.Category))
            {
                var categoryItem = new RadMenuItem
                {
                    Header = category.Key,
                    Tag = category.Key,
                    Icon = new Image
                    {
                        Source = iconService.GetByNameAsImage("directory_add_x16"),
                        Width = 16,
                        Height = 16
                    }
                };

                foreach (var type in category)
                {
                    var menuItem = new RadMenuItem
                    {
                        Header = localizationService.Translate(type.Name),
                        Icon = new Image
                        {
                            Source = iconService.GetByIdAsImage(type.IconId),
                            Width = 16,
                            Height = 16
                        },
                        Tag = type
                    };
                    menuItem.Click += AddDirectoryItemClick;
                    categoryItem.Items.Add(menuItem);
                }

                directoryCategoryMenuItems.Add(categoryItem);
            }

            // Create remove directory command
            removeDirectoryCommand = new RelayCommand((e) =>
            {
                if (SelectedDirectory != null)
                {
                    if (fielStructureService.GetDocuments(model, selectedDirectory.Model, true).Any())
                    {
                        MessageBox.Show(localizationService.Translate("filestructure_delete_notallowed"), localizationService.Translate("filestructure_delete_notallowed_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    SelectedDirectory.RemoveDirectory();

                    SelectedDirectory = null;

                    IsDirty = true;

                    RefreshPath();
                }

            }, (e) => { return selectedDirectory != null; });

            // Create edit metadata command
            editMetaDataCommand = new RelayCommand((e) =>
            {
                if (SelectedDirectory != null)
                {
                    var directoryFieldWindow = new DirectoryFieldWindow();

                    directoryFieldWindow.Initialize(SelectedDirectory.Model, GetStructure());
                    directoryFieldWindow.WindowMode = Framework.UI.WindowMode.Edit;
                    directoryFieldWindow.ShowDialog();
                    return;
                }

            }, (e) => { return selectedDirectory != null; });

            renameDirectoryCommand = new RelayCommand((e) =>
            {
                if(SelectedDirectory != null)
                {
                    directoryTreeView.ContainerFromItemRecursive(directoryTreeView.SelectedItem).BeginEdit();
                    return;
                }
            }, (e) => { return selectedDirectory != null; });

            // Command to assign the workflow 
            assignWorkflowCommand = new RelayCommand((e) =>
            {
                var box = ItemBoxManager.GetItemBoxFromDB("IB_Document_Workflow");
                box.ShowDialog();
                var result = box.GetSelectedItemCell("Guid");
                if (result is Guid guid)
                {
                    selectedDirectory.Model.WorkflowId =guid ;
                    Save();
                    RaisePropertyChanged(nameof(AssignedWorkflow));
                    RaisePropertyChanged(nameof(AssignedWorkflowVisibility));
                }
                
            });

            // Archive from clipboard click
            archiveFromClipboard = new RelayCommand((e) =>
            {
                // Save before archive
                if (IsDirty)
                    Save();

                Helper.ArchiveHelper.ArchiveFromClipboard(model, selectedDirectory.Model);
            }, (e) => { return selectedDirectory != null; });

            // Archive from scanner
            archiveFromScanner = new RelayCommand((e) =>
            {
                // Save before archive
                if (IsDirty)
                    Save();

                Helper.ArchiveHelper.ArchiveFromScanClient(model, selectedDirectory.Model);
            }, (e) => { return selectedDirectory != null; });
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            fielStructureService.Save(GetStructure());
            IsDirty = false;
        }

        /// <summary>
        /// Refresh path information
        /// </summary>
        internal void RefreshPath()
        {
            VisualPathElements.Clear();

            // Get structure object
            var fileStructure = GetStructure();

            var currentItem = fileStructure.Directories.FirstOrDefault(x => x.Id == SelectedDirectory?.Model.Id);
            while (currentItem != null)
            {
                var type = directoryTypeService.Get(currentItem.DirectoryTypeId);

                var label = new System.Windows.Controls.Label();
                label.Content = currentItem.Name;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;

                VisualPathElements.Insert(0, label);

                var image = new Image();
                image.Width = 16;
                image.Height = 16;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.Source = iconService.GetByIdAsImage(type.IconId);

                VisualPathElements.Insert(0, image);

                if (currentItem.Parent != null)
                {
                    currentItem = currentItem.Parent;

                    // If a parent exists, we need an arrow in the left side
                    var arrow = new Image();
                    arrow.Width = 16;
                    arrow.Height = 16;
                    arrow.VerticalAlignment = VerticalAlignment.Center;
                    arrow.HorizontalAlignment = HorizontalAlignment.Center;
                    arrow.Margin = new Thickness(3, 0, 3, 0);
                    arrow.Source = iconService.GetByNameAsImage("filestructure_separator_16x");

                    VisualPathElements.Insert(0, arrow);
                }
                else
                {
                    currentItem = null;
                    break;
                }
            }

            if (fileStructure.StackGuid != null && fileStructure.InstanceDataGuid != null)
            {
                var displayContent = stackService.GetInstanceDataContent((Guid)fileStructure.StackGuid, (Guid)fileStructure.InstanceDataGuid);

                if (VisualPathElements.Any())
                {
                    var arrow = new Image();
                    arrow.Width = 16;
                    arrow.Height = 16;
                    arrow.VerticalAlignment = VerticalAlignment.Center;
                    arrow.HorizontalAlignment = HorizontalAlignment.Center;
                    arrow.Margin = new Thickness(3, 0, 3, 0);
                    arrow.Source = iconService.GetByNameAsImage("filestructure_separator_16x");

                    VisualPathElements.Insert(0, arrow);
                }

                var label = new System.Windows.Controls.Label();
                label.Content = displayContent;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;

                VisualPathElements.Insert(0, label);

                var image = new Image();
                image.Width = 16;
                image.Height = 16;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.Source = iconService.GetByNameAsImage("filestructure_root_16x");

                VisualPathElements.Insert(0, image);
            }

            if(expander.IsExpanded)
            {
                expander.Content = null;
                expander.Content = new DirectoryFieldControl();
                (expander.Content as DirectoryFieldControl).Initialize(SelectedDirectory.Model, GetStructure(), true);
            }
        }

        /// <summary>
        /// Add directory item clicked
        /// </summary>
        /// <param name="sender">Menu instance</param>
        /// <param name="e">Argumetns</param>
        private void AddDirectoryItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var directoryType = ((sender as RadMenuItem).Tag as DirectoryType);

            var container = directoryTreeView.SelectedContainer;

            var directory = new Directory
            {
                DirectoryTypeId = directoryType.Id,
                Name = localizationService.Translate("fs_new_directory_name")
            };

            // Save child directory list
            ObservableCollection<DirectoryViewModel> childrenDirectory = null;

            var directoryViewModel = new DirectoryViewModel(directory, this)
            {
                Parent = SelectedDirectory as IViewModelBase ?? this
            };

            if (SelectedDirectory == null)
            {
                Directories.Add(directoryViewModel);
                childrenDirectory = Directories;
            }
            else
            {
                directory.Parent = SelectedDirectory.Model;
                SelectedDirectory.Directories.Add(directoryViewModel);

                childrenDirectory = SelectedDirectory.Directories;
            }

            SelectedDirectory = directoryViewModel;
            RawDirectories.Add(directoryViewModel);

            // Check if directory is already existing
            var currentName = directoryViewModel.Name;
            int nameCounter = 1;
            while (childrenDirectory.Any(x => x.Name?.ToLower() == currentName.ToLower() && x != directoryViewModel))
            {
                currentName = $"{directoryViewModel.Name} {nameCounter}";
                nameCounter++;
            }

            directoryViewModel.Name = currentName;

            // Expand parent
            if (container?.IsExpanded != null)
                container.IsExpanded = true;

            IsDirty = true;

            RefreshPath();
        }

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model to initialize</param>
        public void Initialize(FileStructure model)
        {
            this.model = model;
            directories = new ObservableCollection<DirectoryViewModel>();
            rawDirectories = new List<DirectoryViewModel>();

            foreach (var directory in model.Directories.Where(x => x.Parent == null))
            {
                var directoryViewModel = new DirectoryViewModel(directory, this)
                {
                    Parent = this
                };

                directories.Add(directoryViewModel);
                rawDirectories.Add(directoryViewModel);
                directoryViewModel.LoadChildren(directory, model.Directories);
            }

            RemoveValidators(nameof(Name));

            // Add template validator
            if (model.IsTemplate)
                AddValidator(nameof(Name), new StringPropertyNoWhiteSpace());

            if (model.StackGuid != null && model.InstanceDataGuid != null)
                rootPath = stackService.GetInstanceDataContent(model.StackGuid.Value, model.InstanceDataGuid.Value);

            IsDirty = false;
            RefreshPath();
        }

        /// <summary>
        /// Get structure instance
        /// </summary>
        /// <returns>Structure instance</returns>
        public FileStructure GetStructure()
        {
            model.Directories.Clear();
            foreach (var directory in rawDirectories)
            {
                // Reset parent directory
                directory.Model.Parent = null;

                // Set parent
                var parent = directory.Parent;
                if (parent is DirectoryViewModel)
                    directory.Model.Parent = (parent as DirectoryViewModel).Model;

                model.Directories.Add(directory.Model);
            }

            return model;
        }

        /// <summary>
        /// Gets the current model instance
        /// </summary>
        public FileStructure Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets whether to use file system sync or not
        /// </summary>
        public bool UseFileSync
        {
            get
            {
                return model.UseFileSync;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.UseFileSync = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets wheter the Expander is expanded or not
        /// </summary>
        public bool ExpanderIsExpanded
        {
            get
            {
                return expanderIsExpanded;
            }
            set
            {
                PropertySetter(value, (newValue) => { expanderIsExpanded = newValue; });
                if (value == true)
                {
                    if (selectedDirectory != null)
                    {
                        expander.Content = new DirectoryFieldControl();
                        (expander.Content as DirectoryFieldControl).Initialize(selectedDirectory.Model, GetStructure(), true);
                    }
                }
                else
                {
                    expander.Content = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current hash for sync
        /// </summary>
        public string SyncHash
        {
            get
            {
                return model.SyncHash;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.SyncHash = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets the server path for file sync
        /// </summary>
        public string SyncPath
        {
            get
            {
                return model.SyncPath;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.SyncPath = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets the structure name
        /// </summary>
        public string Name
        {
            get
            {
                return model.Name;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.Name = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets whether this is a structure template
        /// </summary>
        public bool IsTemplate
        {
            get
            {
                return model.IsTemplate;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.IsTemplate = newValue; });
                RaisePropertyChanged(nameof(SettingVisibility));
            }
        }

        /// <summary>
        /// Gets or sets the selected directory
        /// </summary>
        public DirectoryViewModel SelectedDirectory
        {
            get
            {
                return selectedDirectory;
            }

            set
            {
                selectedDirectory = value;
                RaisePropertyChanged(nameof(SelectedDirectory));
                RaisePropertyChanged(nameof(AssignedWorkflow));
                RaisePropertyChanged(nameof(AssignedWorkflowVisibility));

                RefreshPath();
            }
        }

        /// <summary>
        /// Gets or sets the remove directory command
        /// </summary>
        public ICommand RemoveDirectoryCommand
        {
            get
            {
                return removeDirectoryCommand;
            }

            set
            {
                removeDirectoryCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the edit metadata directory command
        /// </summary>
        public ICommand EditMetaDataCommand
        {
            get
            {
                return editMetaDataCommand;
            }

            set
            {
                editMetaDataCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the rename directory command
        /// </summary>
        public ICommand RenameDirectoryCommand
        {
            get
            {
                return renameDirectoryCommand;
            }
            
            set
            {
                renameDirectoryCommand = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the assign workflow command
        /// </summary>
        public ICommand AssignWorkflowCommand
        {
            get
            {
                return assignWorkflowCommand;
            }
            
            set
            {
                assignWorkflowCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of root directories
        /// </summary>
        public ObservableCollection<DirectoryViewModel> Directories
        {
            get
            {
                return directories;
            }

            set
            {
                directories = value;
            }
        }

        /// <summary>
        /// Gets a list of all directories
        /// </summary>
        public IList<DirectoryViewModel> RawDirectories
        {
            get
            {
                return rawDirectories;
            }
        }

        /// <summary>
        /// Gets or sets the archive from clipboard command
        /// </summary>
        public ICommand ArchiveFromClipboard
        {
            get
            {
                return archiveFromClipboard;
            }

            set
            {
                archiveFromClipboard = value;
            }
        }

        /// <summary>
        /// Gets or sets the archive from scanner command
        /// </summary>
        public ICommand ArchiveFromScanner
        {
            get
            {
                return archiveFromScanner;
            }

            set
            {
                archiveFromScanner = value;
            }
        }

        /// <summary>
        /// Gets a visibility falg based on <see cref="IsTemplate"/>
        /// </summary>
        public Visibility SettingVisibility
        {
            get
            {
                return IsTemplate ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// Gets a visibility falg based on <see cref="IsTemplate"/>
        /// </summary>
        public Visibility TemplateGridVisibility
        {
            get
            {
                return IsTemplate ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ObservableCollection<RadMenuItem> DirectoryCategoryMenuItems
        {
            get
            {
                return directoryCategoryMenuItems;
            }

            set
            {
                directoryCategoryMenuItems = value;
            }
        }

        public ObservableCollection<FrameworkElement> VisualPathElements
        {
            get
            {
                return visualPathElements;
            }

            set
            {
                visualPathElements = value;
            }
        }
        /// <summary>
        /// Gets or sets the assigned workflow
        /// </summary>
        public string AssignedWorkflow
        {
            get
            {
                if (selectedDirectory.Model.WorkflowId == null || selectedDirectory.Model.WorkflowId == Guid.Empty)
                    return "";

                ;
                return localizationService.Translate("filestructure_assigned_workflow") +": " + documentWorkflowContextService.Get((Guid)selectedDirectory.Model.WorkflowId).DisplayName;
            }

        }

        /// <summary>
        /// Gets the assigned workflow visibility 
        /// </summary>
        public Visibility AssignedWorkflowVisibility 
        {
            get
            {
                var flag = (selectedDirectory.Model.WorkflowId == null || selectedDirectory.Model.WorkflowId == Guid.Empty
                    || selectedDirectory.DirectoryType.DirectoryFunction != DirectoryFunctionType.Workflow);
                return flag ? Visibility.Collapsed : Visibility.Visible;
                
            }  
        }
    }
}
