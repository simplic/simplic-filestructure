using Simplic.Localization;
using System.Collections.ObjectModel;
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

        public FileStructureControl()
        {
            InitializeComponent();

            // Subscribe to preview drop event
            DragDropManager.AddPreviewDropHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragEventHandler(OnPreviewDrop), true);
            DragDropManager.AddDragOverHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragEventHandler(OnDragOver), true);

            EventManager.RegisterClassHandler(typeof(RadTreeViewItem), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnTreeViewItemMouseDown), false);

            if (localizationService == null)
                localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
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
                
                if (!draggedDirectory.DirectoryType.EnableDrag || !targetItem.DirectoryType.EnableDrop || targetItem == draggedDirectory || childDirectoryList != null && childDirectoryList.Any(x => x.Name?.ToLower() == draggedDirectory.Name.ToLower() && x != draggedDirectory))
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
                    droppedDirectory.Parent = targetItem;

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

        /// <summary>
        /// Gets the directory tree view
        /// </summary>
        public RadTreeView DirectoryTreeView
        {
            get
            {
                return directoryTreeView;
            }
        }
    }
}
