using Simplic.Framework.DBUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Grid which will be shown next to the file structure treeview
    /// </summary>
    public class FileStructureDocumentGrid : UserControl
    {
        private IntegratedGridView integratedGridView;
        private Guid lastDirectoryId;

        /// <summary>
        /// Directory id binding property
        /// </summary>
        public static readonly DependencyProperty DirectoryIdProperty =
            DependencyProperty.Register("DirectoryId", typeof(Guid), typeof(FileStructureDocumentGrid), new PropertyMetadata(Guid.Empty, DirectoryIdChangedCallback));

        /// <summary>
        /// File structure id property
        /// </summary>
        public static readonly DependencyProperty FileStructureIdProperty =
            DependencyProperty.Register("FileStructureId", typeof(Guid), typeof(FileStructureDocumentGrid), new PropertyMetadata(Guid.Empty));
        
        /// <summary>
        /// Directory id changed callback
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DirectoryIdChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as FileStructureDocumentGrid;
            if (grid.isLoaded)
            {
                if (grid.lastDirectoryId != (Guid)e.NewValue && (Guid)e.NewValue != Guid.Empty)
                {
                    grid.integratedGridView.EmbeddedGridView?.SetPlaceholder("[DirectoryId]", e.NewValue.ToString());
                    grid.integratedGridView.EmbeddedGridView?.SetPlaceholder("[FileStructureId]", grid.FileStructureId.ToString());
                    grid.integratedGridView.RefreshData();

                    grid.lastDirectoryId = (Guid)e.NewValue;
                }
                if ((Guid)e.NewValue == Guid.Empty)
                {
                    grid.integratedGridView.CancelLoading();
                    grid.integratedGridView.EmbeddedGridView.Clear();

                    grid.lastDirectoryId = (Guid)e.NewValue;
                }
            }
        }

        private bool isLoaded;

        /// <summary>
        /// Initialize grid
        /// </summary>
        public FileStructureDocumentGrid()
        {
            integratedGridView = new IntegratedGridView();
            Content = integratedGridView;

            integratedGridView.LoadConfiguration("Grid_Document_FileStructure");

            // Profile changed
            integratedGridView.SelectedProfileChanged += (s, e) =>
            {
                lastDirectoryId = DirectoryId;
                integratedGridView.EmbeddedGridView?.SetPlaceholder("[DirectoryId]", DirectoryId.ToString());
                integratedGridView.EmbeddedGridView?.SetPlaceholder("[FileStructureId]", FileStructureId.ToString());
            };

            // Control loaded
            Loaded += (s, e) =>
            {
                if (!isLoaded)
                {
                    isLoaded = true;
                }
            };
        }

        /// <summary>
        /// Gets or sets the binded directory id
        /// </summary>
        public Guid DirectoryId
        {
            get { return (Guid)GetValue(DirectoryIdProperty); }
            set { SetValue(DirectoryIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the file structure id
        /// </summary>
        public Guid FileStructureId
        {
            get { return (Guid)GetValue(FileStructureIdProperty); }
            set { SetValue(FileStructureIdProperty, value); }
        }
    }
}
