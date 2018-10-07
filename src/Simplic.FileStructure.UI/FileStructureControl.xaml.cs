﻿using System;
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
        public FileStructureControl()
        {
            InitializeComponent();

            // Subscribe to preview drop event
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
                }

                // Reset selected item
                droppedDirectory.StructureViewModel.SelectedDirectory = droppedDirectory;

                treeView.Dispatcher.BeginInvoke(new Action(() => {
                    var treeViewItem = treeView.ContainerFromItemRecursive(droppedDirectory);

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
    }
}
