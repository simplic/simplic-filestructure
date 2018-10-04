using Simplic.Framework.DBUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Grid which will be shown next to the file structure treeview
    /// </summary>
    public class FileStructureDocumentGrid : IntegratedGridView
    {
        /// <summary>
        /// Directory id binding property
        /// </summary>
        public static readonly DependencyProperty DirectoryIdProperty =
            DependencyProperty.Register("DirectoryId", typeof(Guid), typeof(FileStructureDocumentGrid), new PropertyMetadata(Guid.Empty, DirectoryIdChangedCallback));

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
                grid.EmbeddedGridView?.SetPlaceholder("[DirectoryId]", e.NewValue.ToString());
                grid.RefreshData();
            }
        }

        private bool isLoaded;

        /// <summary>
        /// Initialize grid
        /// </summary>
        public FileStructureDocumentGrid()
        {
            // Profile changed
            SelectedProfileChanged += (s, e) =>
            {
                EmbeddedGridView?.SetPlaceholder("[DirectoryId]", DirectoryId.ToString());
            };

            // Control loaded
            Loaded += (s, e) =>
            {
                if (!isLoaded)
                {
                    this.RefreshData();
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
    }
}
