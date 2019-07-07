using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using Simplic.Localization;
using Simplic.Icon;
using System.Windows.Media.Imaging;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Directory viewmodel
    /// </summary>
    public class DirectoryViewModel : ExtendableViewModel, IDirectoryBaseViewModel
    {
        private ObservableCollection<DirectoryViewModel> directories;
        private Directory model;
        private FileStructureViewModel structureViewModel;
        private DirectoryType directoryType;
        private BitmapImage iconImage;
        private String tooltip;


        private readonly ILocalizationService localizationService;
        private readonly IIconService iconService;
        private readonly IDirectoryTypeService directoryTypeService;

        private readonly IDirectoryClassificationFieldService directoryTypeFieldService;
        private readonly IDirectoryFieldService directoryFieldService;
        private readonly IFieldTypeService fieldTypeService;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model"></param>
        public DirectoryViewModel(Directory model, FileStructureViewModel structureViewModel)
        {
            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
            directoryTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            iconService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IIconService>();

            directoryTypeFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationFieldService>();
            directoryFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryFieldService>();
            fieldTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFieldTypeService>();


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
            foreach (var directory in directoryModels.Where(x => x.Parent?.Id == parent?.Id))
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
                try
                {
                    if (string.IsNullOrWhiteSpace(value) || value.Contains("\\") || value.Contains("/"))
                        throw new Exception();

                    // Demo path, to check whether the path is correct
                    Path.GetFullPath($"C:\\{value}");
                }
                catch
                {
                    System.Windows.MessageBox.Show(localizationService.Translate("invalid_path_messagebox"), localizationService.Translate("invalid_path_messagebox_title"), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    return;
                }

                // Check whether a directory with the same name already exists on the same level
                if (Parent is IDirectoryBaseViewModel && Parent != null)
                {
                    var directoryParent = Parent as IDirectoryBaseViewModel;
                    if (directoryParent.Directories.Any(x => x.Name?.ToLower() == value?.ToLower() && x != this))
                    {
                        System.Windows.MessageBox.Show(localizationService.Translate("fs_directory_already_exists"), localizationService.Translate("fs_directory_already_exists_title"), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        return;
                    }
                }

                PropertySetter(value, (newValue) => { model.Name = newValue; });
                structureViewModel.RaisePropertyChanged("SelectedPath");
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

        /// <summary>
        /// Gets the current directory type
        /// </summary>
        public DirectoryType DirectoryType
        {
            get
            {
                if (directoryType == null)
                    directoryType = directoryTypeService.Get(model.DirectoryTypeId);

                return directoryType;
            }
        }

        /// <summary>
        /// Gets the icon image
        /// </summary>
        public BitmapImage IconImage
        {
            get
            {
                if (iconImage == null)
                        iconImage = iconService.GetByIdAsImage(DirectoryType.IconId);

                return iconImage;
            }
        }

        public String Tooltip
        {
            get
            {
                try
                {

                    if (model.DirectoryClassification == null)
                        return "Keine Klassifikation gesetzt";

                    tooltip = "Type: " + model.DirectoryClassification.Name + Environment.NewLine + Environment.NewLine;

                    var dirFieldTypes = directoryTypeFieldService.GetByDirectoryClassificationId(model.DirectoryClassification.Id);

                    foreach (var dirFieldType in dirFieldTypes)
                    {
                        var fieldType = fieldTypeService.Get(dirFieldType.FieldTypeId);
                        var dirField = directoryFieldService.Get(model.Id, dirFieldType.FieldTypeId);

                        tooltip += $"{fieldType.Name}: ";

                        switch (fieldType.Datatype)
                        {
                            case "DateTime":
                                tooltip += dirField.DateValue + Environment.NewLine;
                                break;
                            case "string":
                                tooltip += dirField.StringValue + Environment.NewLine;
                                break;
                            case "int":
                                tooltip += dirField.NumericValue + Environment.NewLine;
                                break;
                            case "bool":
                                tooltip += dirField.BooleanValue + Environment.NewLine;
                                break;
                        }
                    }
                    return tooltip;
                }
                catch { return "Error loading Tooltip"; }
            }
        }
    }
}
