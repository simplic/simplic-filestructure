using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// File structure editor viewmodel
    /// </summary>
    public class FileStructureViewModel : ExtendableViewModel, Studio.UI.IWindowViewModel<FileStructure>
    {
        private ObservableCollection<DirectoryViewModel> directories;
        private FileStructure model;

        /// <summary>
        /// Create view model
        /// </summary>
        public FileStructureViewModel()
        {
            
        }

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model to initialize</param>
        public void Initialize(FileStructure model)
        {
            this.model = model;
            directories = new ObservableCollection<DirectoryViewModel>();

            foreach (var directory in model.Directories)
            {
                directories.Add(new DirectoryViewModel(directory)
                {
                    Parent = this
                });
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
            }
        }
    }
}
