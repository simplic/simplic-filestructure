using Simplic.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.DragDrop;

namespace Simplic.FileStructure.UI
{
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

                if (targetItem == draggedDirectory || childDirectoryList != null && childDirectoryList.Any(x => x.Name?.ToLower() == draggedDirectory.Name.ToLower() && x != draggedDirectory))
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
                    // Disable drag and drop over the same item
                    if (targetItem.Model.Id == droppedDirectory.Model.Id)
                    {
                        e.Handled = false;
                        return;
                    }

                    targetItem.Directories.Add(droppedDirectory);
                    droppedDirectory.Parent = targetItem;

                    // Expand target item
                    options.DropTargetItem.IsExpanded = true;
                }

                // Reset selected item
                droppedDirectory.StructureViewModel.SelectedDirectory = droppedDirectory;

                treeView.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var treeViewItem = GetTreeViewItem(treeView, droppedDirectory);

                    // TODO: This is null, don't know why
                    if (treeViewItem != null)
                    {
                        treeViewItem.Focus();
                        Keyboard.Focus(treeViewItem);
                    }
                }), DispatcherPriority.Normal);

                // Drag/Drop already done above
                e.Handled = true;
            }
        }

        /// <summary>
        /// Get treeview item by data context
        /// </summary>
        /// <param name="treeView">Treeview instance</param>
        /// <param name="viewModel">Datacontext of the treeview</param>
        /// <returns>Tree view item if found</returns>
        private static RadTreeViewItem GetTreeViewItem(RadTreeView treeView, DirectoryViewModel viewModel)
        {
            var items = GetAllItemContainers(treeView);
            return items.FirstOrDefault(x => x.DataContext == viewModel);
        }

        /// <summary>
        /// Get a collection of rad tree view items.
        /// </summary>
        /// <param name="itemsControl"></param>
        /// <returns></returns>
        private static Collection<RadTreeViewItem> GetAllItemContainers(ItemsControl itemsControl)
        {
            Collection<RadTreeViewItem> allItems = new Collection<RadTreeViewItem>();
            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                // try to get the item Container  
                RadTreeViewItem childItemContainer = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;
                // the item container maybe null if it is still not generated from the runtime  
                if (childItemContainer != null)
                {
                    allItems.Add(childItemContainer);
                    Collection<RadTreeViewItem> childItems = GetAllItemContainers(childItemContainer);
                    foreach (RadTreeViewItem childItem in childItems)
                    {
                        allItems.Add(childItem);
                    }
                }
            }
            return allItems;
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
