using Simplic.Framework.UI;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// File structure editor viewmodel
    /// </summary>
    public class FileStructureViewModel : ViewModelBase, Studio.UI.IWindowViewModel<FileStructure>, IDirectoryBaseViewModel
    {
        private ObservableCollection<DirectoryViewModel> directories;
        private IList<DirectoryViewModel> rawDirectories;

        private DirectoryViewModel selectedDirectory;
        private FileStructure model;
        private ICommand addDirectoryCommand;
        private ICommand removeDirectoryCommand;

        /// <summary>
        /// Create view model
        /// </summary>
        public FileStructureViewModel()
        {
            addDirectoryCommand = new RelayCommand((e) => 
            {
                var directory = new Directory();
                directory.Name = "new_directory_name";

                var directoryViewModel = new DirectoryViewModel(directory, this)
                {
                    Parent = SelectedDirectory as IViewModelBase ?? this
                };

                if (SelectedDirectory == null)
                {
                    Directories.Add(directoryViewModel);
                }
                else
                {
                    directory.Parent = SelectedDirectory.Model;
                    SelectedDirectory.Directories.Add(directoryViewModel);
                }

                SelectedDirectory = directoryViewModel;
            });

            removeDirectoryCommand = new RelayCommand((e) =>
            {
                if (SelectedDirectory != null)
                {
                    SelectedDirectory.RemoveDirectory();                   

                    SelectedDirectory = null;
                }

            }, (e) => { return selectedDirectory != null; });
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
                // PropertySetter(value, (newValue) => { model.UseFileSync = newValue; });
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
                // PropertySetter(value, (newValue) => { model.SyncHash = newValue; });
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
                // PropertySetter(value, (newValue) => { model.SyncPath = newValue; });
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
                // PropertySetter(value, (newValue) => { model.Name = newValue; });
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
                // PropertySetter(value, (newValue) => { model.IsTemplate = newValue; });
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
            }
        }

        /// <summary>
        /// Gets or sets the add directory command
        /// </summary>
        public ICommand AddDirectoryCommand
        {
            get
            {
                return addDirectoryCommand;
            }

            set
            {
                addDirectoryCommand = value;
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
    }
}
