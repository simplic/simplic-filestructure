using Simplic.Base;
using Simplic.DataStack;
using Simplic.Framework.DBUI;
using Simplic.Framework.DocumentProcessing.Outlook;
using Simplic.Localization;
using Simplic.UI.GridView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.DragDrop;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Mouse double clicked within the tree view
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="args">Arguments</param>
    public delegate void TreeViewMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs args);

    /// <summary>
    /// Interaction logic for FileStructureEditor.xaml
    /// </summary>
    public partial class FileStructureControl : UserControl
    {
        private static ILocalizationService localizationService;
        private static IStackService stackService;
        private static IFileStructureService fielStructureService;
        private static IFileStructureDocumentPathService fileStructureDocumentPathService;


        /// <summary>
        /// Initialize control
        /// </summary>
        public FileStructureControl()
        {
            InitializeComponent();

            stackService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IStackService>();
            fielStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            fileStructureDocumentPathService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();

            // Subscribe to preview drop event
            DragDropManager.AddPreviewDropHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragEventHandler(OnPreviewDrop), true);
            DragDropManager.AddDragOverHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragEventHandler(OnDragOver), true);
            DragDropManager.AddDropHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragEventHandler(OnDrop), true);
            DragDropManager.AddDragInitializeHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragInitializeEventHandler(OnDraginitialize), true);

            EventManager.RegisterClassHandler(typeof(RadTreeViewItem), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnTreeViewItemMouseDown), false);

            if (localizationService == null)
                localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
        }

        /// <summary>
        /// Initialize control and fill data
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        public FileStructureViewModel Initialize(FileStructure fileStructure)
        {
            var viewModel = new FileStructureViewModel(directoryTreeView, expanderMetadata);
            viewModel.Initialize(fileStructure);

            DataContext = viewModel;

            if (!fileStructure.IsTemplate)
            {
                // Initialize grid
                var stackId = stackService.GetStackId("STACK_Document");

                searchOverviewGrid.FillOnProfileChanged = false;
                searchOverviewGrid.SetBlobSettings(true, true);
                searchOverviewGrid.SetConfig("Grid_Document_FileStructure_Overview", "Search_Documents", stackId, Guid.Empty, new Guid[] { });

                // Set file-structure variable
                searchOverviewGrid.GridView.SelectedProfileChanged += (s, e) =>
                {
                    searchOverviewGrid.GridView.EmbeddedGridView.SetPlaceholder("[FileStructureId]", fileStructure.Id.ToString());
                };

                searchOverviewGrid.GridView.Loaded += (s, e) =>
                {
                    searchOverviewGrid.GridView.EmbeddedGridView.SetPlaceholder("[FileStructureId]", fileStructure.Id.ToString());
                };
            }

            // Reset dirty status
            viewModel.IsDirty = false;

            return viewModel;
        }

        /// <summary>
        /// Item mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void OnTreeViewItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewitem = sender as RadTreeViewItem;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                treeViewitem.IsSelected = true;

                // Expand item
                treeViewitem.IsExpanded = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Tree view mouse double clicked
        /// </summary>
        public event TreeViewMouseDoubleClickEventHandler TreeViewMouseDoubleClick;

        /// <summary>
        /// Mouse double clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeViewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewMouseDoubleClick?.Invoke(sender, e);
        }

        /// <summary>
        /// Initialize directory dragging 
        /// </summary>
        /// <param name="sener"></param>
        /// <param name="e"></param>
        private static void OnDraginitialize(object sener, Telerik.Windows.DragDrop.DragInitializeEventArgs e)
        {

            var options = DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) as TreeViewDragDropOptions;
            bool isProtected = false;
            //Check if there is any child of the item protected
            var draggedDirectory = options.DraggedItems.OfType<DirectoryViewModel>().FirstOrDefault();
            var guids = new List<Guid>();
            var directoriesToCheck = new List<DirectoryViewModel>();
            directoriesToCheck.Add(draggedDirectory);


            while (directoriesToCheck.Any())
            {
                var innerDirectories = new List<DirectoryViewModel>();

                foreach (var subDirectory in directoriesToCheck)
                {
                    guids.Add(subDirectory.Model.Id);
                    innerDirectories.AddRange(subDirectory.Directories);
                }

                directoriesToCheck.Clear();
                directoriesToCheck.AddRange(innerDirectories);
            }

            if (fileStructureDocumentPathService.IsProtected(guids))
            {
                MessageBox.Show(localizationService.Translate("filestructure_drag_protected"), localizationService.Translate("filestructure_delete_notallowed_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                e.Data = null;
                e.DragVisual = null;
                e.Handled = true;
            }

            //Checks if the folder is assigned to a workflow
            if (draggedDirectory.Model.WorkflowId.HasValue)
            {
                MessageBox.Show(localizationService.Translate("filestructure_drag_protected_workflow"), localizationService.Translate("filestructure_delete_notallowed_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                e.Data = null;
                e.DragVisual = null;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Drop event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            // Find RadTreeViewItem
            var item = e.OriginalSource as RadTreeViewItem;
            if (e.OriginalSource != null && e.OriginalSource is DependencyObject)
            {
                var dependencySource = (DependencyObject)e.OriginalSource;
                item = Framework.UI.WPFVisualTreeHelper.FindParent<RadTreeViewItem>(dependencySource);
            }

            var targetDirectory = item?.DataContext as DirectoryViewModel;

            // Save target filestructure before drop action
            if (targetDirectory.StructureViewModel.IsDirty)
                targetDirectory.StructureViewModel.Save();
            //Check if the the folder is a workflow folder and has a workflow assigned 
            if (IsDirectoryWorkflow(targetDirectory))
                return;
            // File drag & drop
            DataObject dataObject = (e.Data as DataObject);
            if (dataObject != null && dataObject.ContainsFileDropList())
            {
                foreach (var file in dataObject.GetFileDropList())
                {
                    Helper.ArchiveHelper.ArchiveFile(targetDirectory.StructureViewModel.Model, targetDirectory.Model, file);
                }
            }
            else if (dataObject != null && dataObject.GetData("FileGroupDescriptorW") != null)
            {
                var outlookDataObject = new OutlookDataObject(dataObject);

                string[] filenames = (string[])outlookDataObject.GetData("FileGroupDescriptorW");
                var fileStreams = (MemoryStream[])outlookDataObject.GetData("FileContents");

                string directory = GlobalSettings.AppDataPath + "\\Temp\\Blobs\\";
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                for (int fileIndex = 0; fileIndex < filenames.Length; fileIndex++)
                {
                    //use the fileindex to get the name and data stream
                    string filename = filenames[fileIndex];
                    MemoryStream filestream = fileStreams[fileIndex];

                    //save the file stream using its name to the application path
                    FileStream outputStream = File.Create(directory + filename);
                    filestream.WriteTo(outputStream);
                    outputStream.Close();

                    if (filename.ToLower().EndsWith(".msg"))
                    {
                        try
                        {
                            OutlookStorage.Message msg = new OutlookStorage.Message(directory + filename);
                            DateTime receiveDateTime = msg.ReceivedDate;
                            msg.Dispose();

                            File.SetLastWriteTime(directory + filename, receiveDateTime);
                        }
                        catch (Exception ex)
                        {
                            Log.LogManagerInstance.Instance.Error(string.Format(@"Invalid mail format: {0}", filename), ex);
                        }
                    }
                    else
                    {
                        File.SetLastWriteTime(directory + filename, DateTime.Now);
                    }

                    Helper.ArchiveHelper.ArchiveFile(targetDirectory.StructureViewModel.Model, targetDirectory.Model, directory + filename);
                }
            }
            else if (dataObject != null && dataObject.GetData(typeof(GridViewPayload)) != null)
            {
                var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();
                var localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
                var payload = dataObject.GetData(typeof(GridViewPayload)) as GridViewPayload;

                foreach (var path in payload.DataObjects.OfType<FileStructureDocumenPath>())
                {
                    var copyMode = Keyboard.Modifiers == ModifierKeys.Shift;

                    if (copyMode)
                    {
                        var newPath = new FileStructureDocumenPath
                        {
                            DirectoryGuid = targetDirectory.Model.Id,
                            FileStructureGuid = targetDirectory.StructureViewModel.Model.Id,
                            DocumentGuid = path.DocumentGuid
                        };

                        service.Save(newPath);
                    }
                    else
                    {
                        if (path.IsProtectedPath)
                        {
                            MessageBox.Show(localizationService.Translate("filestructure_path_protected"), localizationService.Translate("filestructure_path_protected_title"), MessageBoxButton.OK, MessageBoxImage.Information);
                            continue;
                        }

                        path.DirectoryGuid = targetDirectory.Model.Id;
                        path.FileStructureGuid = targetDirectory.StructureViewModel.Model.Id;

                        service.Save(path);
                    }
                }

                // Refresh grid
                if (payload.Grid is CursorGridViewControl)
                    (payload.Grid as CursorGridViewControl).RefreshData();
            }

            // Save target filestructure before drop action
            targetDirectory.StructureViewModel.Save();
            var directoriesToCheck = new List<DirectoryViewModel>();
            directoriesToCheck.Add(targetDirectory);
            //Recalculating the path through the save method 
            while (directoriesToCheck.Any())
            {
                var innerDirectories = new List<DirectoryViewModel>();

                foreach (var subDirectory in directoriesToCheck)
                {
                    var guid = subDirectory.Model.Id;
                    var list = fileStructureDocumentPathService.GetByDirectoryId(guid);
                    foreach (FileStructureDocumenPath fileStructureDocumenPath in list)
                    {
                        fileStructureDocumentPathService.Save(fileStructureDocumenPath);
                    }

                    innerDirectories.AddRange(subDirectory.Directories);
                }

                directoriesToCheck.Clear();
                directoriesToCheck.AddRange(innerDirectories);
            }
        }

        /// <summary>
        /// Drag over handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var options = DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) as TreeViewDragDropOptions;
            if (options != null)
            {
                var draggedDirectory = options.DraggedItems.OfType<DirectoryViewModel>().FirstOrDefault();
                var targetItem = options?.DropTargetItem?.DataContext as DirectoryViewModel;


                ObservableCollection<DirectoryViewModel> childDirectoryList;

                // Add to new parent
                if (targetItem == null)
                {
                    childDirectoryList = draggedDirectory.StructureViewModel.Directories;
                }
                else
                {
                    childDirectoryList = targetItem.Directories;
                }

                if (!draggedDirectory.DirectoryType.EnableDrag ||
                    !targetItem.DirectoryType.EnableDrop ||
                    targetItem == draggedDirectory ||
                    childDirectoryList != null && childDirectoryList.Any(x => x.Name?.ToLower() == draggedDirectory.Name.ToLower() && x != draggedDirectory))
                {
                    options.DropAction = DropAction.None;
                    e.Effects = DragDropEffects.None;
                    options.UpdateDragVisual();
                }
                else
                {
                    options.DropAction = DropAction.Move;
                    options.DropPosition = DropPosition.Inside;
                    e.Effects = DragDropEffects.Move;
                    options.UpdateDragVisual();
                }
            }
        }

        /// <summary>
        /// Preview drop handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPreviewDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)

        {
            var options = DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) as TreeViewDragDropOptions;
            var treeView = sender as Telerik.Windows.Controls.RadTreeView;

            if (options != null)
            {
                var droppedDirectory = options.DraggedItems.OfType<DirectoryViewModel>().FirstOrDefault();
                //Check if the the folder is a workflow folder and has a workflow assigned
                if (IsDirectoryWorkflow(droppedDirectory))
                    return;

                var targetItem = options?.DropTargetItem?.DataContext as DirectoryViewModel;

                // Remove from parent
                (droppedDirectory.Parent as IDirectoryBaseViewModel).Directories.Remove(droppedDirectory);

                // Add to new parent
                if (targetItem == null)
                {
                    droppedDirectory.StructureViewModel.Directories.Add(droppedDirectory);
                    droppedDirectory.Parent = droppedDirectory.StructureViewModel;
                }
                else
                {
                    targetItem.Directories.Add(droppedDirectory);

                    // Drag and drop was in the same directory hierarchy
                    if (IsInSamePath(droppedDirectory.StructureViewModel.Model, targetItem.Model, droppedDirectory.Model))
                    {
                        // Switch child/parent of target item
                        var oldParent = targetItem.Parent;
                        targetItem.Parent = droppedDirectory.Parent;
                        targetItem.Model.Parent = droppedDirectory.Model.Parent;

                        var directoryBase = targetItem.Parent as IDirectoryBaseViewModel;

                        if (directoryBase != null)
                            directoryBase.Directories.Add(targetItem);

                        var oldDirectoryBase = oldParent as IDirectoryBaseViewModel;
                        if (oldDirectoryBase != null)
                            oldDirectoryBase.Directories.Remove(targetItem);
                    }

                    droppedDirectory.Parent = targetItem;
                    droppedDirectory.Model.Parent = targetItem.Model;

                    // Expand target item
                    options.DropTargetItem.IsExpanded = true;
                }

                // Reset selected item
                droppedDirectory.StructureViewModel.SelectedDirectory = droppedDirectory;

                // Drag/Drop already done above
                // So cancel it internally, to prevent from adding new items twice
                options.DropAction = DropAction.None;
                e.Effects = DragDropEffects.None;
            }
        }

        private static bool IsDirectoryWorkflow(DirectoryViewModel directory)
        {
            //Guid of the type workflow folder
            var workflowGuid = Guid.Parse("F3F2BF83-5ACD-4221-BAA1-5138ED5D9769");

            if (directory.Model.DirectoryTypeId.Equals(workflowGuid))
            {
                if (!directory.Model.WorkflowId.HasValue)
                {
                    MessageBox.Show(localizationService.Translate("filestructure_workflow_not_assigned"), 
                        localizationService.Translate("filestructure_drag_protected_workflow"), MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
            }
            return false;
        }

        private static bool IsInSamePath(FileStructure fileStructure, Directory startDirectory, Directory directoryToCheck)
        {
            var currentItem = fileStructure.Directories.FirstOrDefault(x => x.Id == startDirectory.Id);
            while (currentItem != null)
            {
                if (currentItem == directoryToCheck)
                    return true;

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

            return false;
        }

        /// <summary>
        /// Preview mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && !(e.OriginalSource is Image) && !(e.OriginalSource is TextBlock))
            {
                ViewModel.SelectedDirectory = null;
            }
        }

        /// <summary>
        /// Gets the current datacontext as <see cref="FileStructureViewModel"/>
        /// </summary>
        public FileStructureViewModel ViewModel
        {
            get
            {
                return DataContext as FileStructureViewModel;
            }
        }
    }
}
