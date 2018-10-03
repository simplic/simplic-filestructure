using System;
using System.Collections.Generic;
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
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.DragDrop;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Interaction logic for FileStructureEditor.xaml
    /// </summary>
    public partial class FileStructureControl : UserControl
    {
        public FileStructureControl()
        {
            InitializeComponent();

            DragDropManager.AddPreviewDropHandler(directoryTreeView, new Telerik.Windows.DragDrop.DragEventHandler(OnPreviewDrop), true);
        }

        /// <summary>
        /// Preview drop handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPreviewDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var options = DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) as TreeViewDragDropOptions;
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
                }

                // Reset selected item
                droppedDirectory.StructureViewModel.SelectedDirectory = droppedDirectory;

                // Drag/Drop already done above
                e.Handled = true;
            }
        }
    }
}
