using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Simplic.UI.MVC;
using System.Windows.Data;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Directory viewmodel
    /// </summary>
    public class DirectoryViewModel : ViewModelBase, IDirectoryBaseViewModel
    {
        private ObservableCollection<DirectoryViewModel> directories;
        private Directory model;
        private FileStructureViewModel structureViewModel;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model"></param>
        public DirectoryViewModel(Directory model, FileStructureViewModel structureViewModel)
        {
            directories = new ObservableCollection<DirectoryViewModel>();

            this.structureViewModel = structureViewModel;
            this.model = model;
        }

        /// <summary>
        /// Load subdirectories
        /// </summary>
        /// <param name="parent">Parent directory</param>
        /// <param name="directoryModels">Complete directory list</param>
        internal void LoadChildren(Directory parent, IList<Directory> directoryModels)
        {
            foreach (var directory in directoryModels.Where(x => x.Parent == parent))
            {
                var directoryViewModel = new DirectoryViewModel(directory, structureViewModel)
                {
                    // Set FileStructureViewModel as parent
                    Parent = this
                };

                directories.Add(directoryViewModel);
                structureViewModel.RawDirectories.Add(directoryViewModel);
                directoryViewModel.LoadChildren(directory, directoryModels);
            }
        }

        /// <summary>
        /// Remove current directory
        /// </summary>
        public void RemoveDirectory()
        {
            // Create copy first
            var directoryCopy = Directories.ToList();

            foreach (var directory in directoryCopy)
            {
                directory.RemoveDirectory();
                directories.Remove(directory);
            }


            (Parent as IDirectoryBaseViewModel).Directories.Remove(this);
            structureViewModel.RawDirectories.Remove(this);
        }

        /// <summary>
        /// Gets the directory model
        /// </summary>
        public Directory Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets the directory name
        /// </summary>
        public string Name
        {
            get
            {
                return model.Name;
            }
            set
            {
                model.Name = value;
                RaisePropertyChanged(nameof(Name));
                // PropertySetter(value, (newValue) => { model.Name = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets a list of subdirectories
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
        /// Gets the parent structure viewmodel
        /// </summary>
        public FileStructureViewModel StructureViewModel
        {
            get
            {
                return structureViewModel;
            }
        }

        // public override Uri ImageUrl
        // {
        //     get
        //     {
        //         return null;
        //     }
        // }
    }
}
