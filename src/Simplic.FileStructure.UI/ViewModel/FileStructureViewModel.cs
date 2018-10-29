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
        private ObservableCollection<RadMenuItem> directoryTypeMenuItems;
        private IList<DirectoryViewModel> rawDirectories;

        private DirectoryViewModel selectedDirectory;
        private FileStructure model;
        private RadTreeView directoryTreeView;
        private string selectedPath;

        private ICommand removeDirectoryCommand;
        private ICommand archiveFromClipboard;
        private ICommand archiveFromScanner;

        private readonly ILocalizationService localizationService;
        private readonly IIconService iconService;
        private readonly IDirectoryTypeService directoryTypeService;

        /// <summary>
        /// Create view model
        /// </summary>
        public FileStructureViewModel(RadTreeView directoryTreeView)
        {
            this.directoryTreeView = directoryTreeView;

            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
            directoryTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            iconService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IIconService>();

            directoryTypeMenuItems = new ObservableCollection<RadMenuItem>();
            foreach (var type in directoryTypeService.GetAll())
            {
                var menuItem = new RadMenuItem
                {
                    Header = localizationService.Translate(type.Name),
                    Icon = new Image
                    {
                        Source = iconService.GetByNameAsImage(type.IconName),
                        Width = 16,
                        Height = 16
                    },
                    Tag = type
                };

                directoryTypeMenuItems.Add(menuItem);
                menuItem.Click += AddDirectoryItemClick;
            }

            // Create remove directory command
            removeDirectoryCommand = new RelayCommand((e) =>
            {
                if (SelectedDirectory != null)
                {
                    SelectedDirectory.RemoveDirectory();

                    SelectedDirectory = null;

                    IsDirty = true;
                }

            }, (e) => { return selectedDirectory != null; });

            // Archive from clipboard click
            archiveFromClipboard = new RelayCommand((e) =>
            {
                Helper.ArchiveHelper.ArchiveFromClipboard(model, selectedDirectory.Model);
            }, (e) => { return selectedDirectory != null; });

            // Archive from scanner
            archiveFromScanner = new RelayCommand((e) =>
            {
                Helper.ArchiveHelper.ArchiveFromScanClient(model, selectedDirectory.Model);
            }, (e) => { return selectedDirectory != null; });            
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

            IsDirty = false;
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
                RaisePropertyChanged(nameof(SelectedPath));
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
        /// Gets or sets the selected path
        /// </summary>
        public string SelectedPath
        {
            get
            {
                if (SelectedDirectory == null)
                {
                    selectedPath = "/";
                    return selectedPath;
                }

                var parent = SelectedDirectory?.Parent as DirectoryViewModel;
                selectedPath = $"/{SelectedDirectory.Name}";

                while (parent != null)
                {
                    selectedPath = $"/{parent.Name}" + selectedPath;
                    parent = parent.Parent as DirectoryViewModel;
                }

                return selectedPath;
            }

            set
            {
                selectedPath = value;
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

        public ObservableCollection<RadMenuItem> DirectoryTypeMenuItems
        {
            get
            {
                return directoryTypeMenuItems;
            }

            set
            {
                directoryTypeMenuItems = value;
            }
        }
    }
}
