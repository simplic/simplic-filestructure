using Simplic.DataStack;
using Simplic.Icon;
using Simplic.UI.MVC;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Document path viewmodel
    /// </summary>
    public class DocumentPathViewModel : ViewModelBase
    {
        private FileStructureDocumenPath path;
        private ObservableCollection<FrameworkElement> visualPathElements;

        private readonly IFileStructureService fileStructureService;
        private readonly IDirectoryTypeService directoryTypeService;
        private readonly IIconService iconService;
        private readonly IStackService stackService;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="path">Path instance</param>
        /// <param name="fileStructureService">File structure service</param>
        /// <param name="directoryTypeService">Directory type service</param>
        /// <param name="iconService">Icon service</param>
        /// <param name="stackService">Stack service</param>
        public DocumentPathViewModel(FileStructureDocumenPath path, IFileStructureService fileStructureService, IDirectoryTypeService directoryTypeService, IIconService iconService, IStackService stackService)
        {
            this.path = path;
            this.fileStructureService = fileStructureService;
            this.iconService = iconService;
            this.directoryTypeService = directoryTypeService;
            this.stackService = stackService;

            VisualPathElements = new ObservableCollection<FrameworkElement>();

            RefreshPath();
        }

        /// <summary>
        /// Refresh path information
        /// </summary>
        internal void RefreshPath()
        {
            Model.Path = "";
            VisualPathElements.Clear();
            var fileStructure = fileStructureService.Get(Model.FileStructureGuid);

            if (fileStructure != null)
            {
                var currentItem = fileStructure.Directories.FirstOrDefault(x => x.Id == path.DirectoryGuid);
                while (currentItem != null)
                {
                    var type = directoryTypeService.Get(currentItem.DirectoryTypeId);

                    var label = new Label();
                    label.Content = currentItem.Name;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Center;

                    VisualPathElements.Insert(0, label);

                    var image = new Image();
                    image.Width = 16;
                    image.Height = 16;
                    image.VerticalAlignment = VerticalAlignment.Center;
                    image.HorizontalAlignment = HorizontalAlignment.Center;
                    image.Source = iconService.GetByNameAsImage(type.IconName);

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

                    var arrow = new Image();
                    arrow.Width = 16;
                    arrow.Height = 16;
                    arrow.VerticalAlignment = VerticalAlignment.Center;
                    arrow.HorizontalAlignment = HorizontalAlignment.Center;
                    arrow.Margin = new Thickness(3, 0, 3, 0);
                    arrow.Source = iconService.GetByNameAsImage("filestructure_separator_16x");

                    VisualPathElements.Insert(0, arrow);

                    var label = new Label();
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
            }
        }

        /// <summary>
        /// Gets the current document path model
        /// </summary>
        public FileStructureDocumenPath Model
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// Gets or sets the visual path elements
        /// </summary>
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
        /// Gets or sets whether this is the primary file structure path
        /// </summary>
        public bool IsPrimary
        {
            get
            {
                return Model.IsPrimaryPath;
            }

            set
            {
                if (value == true)
                {
                    var parent = Parent as DocumentPathOverViewViewModel;
                    foreach (var path in parent.Paths)
                    {
                        if (path != this)
                        {
                            // Use model here!
                            path.Model.IsPrimaryPath = false;
                            path.RaisePropertyChanged(nameof(IsPrimary));
                        }
                    }

                    Model.IsPrimaryPath = value;
                    RaisePropertyChanged(nameof(IsPrimary));
                }
            }
        }
    }
}
