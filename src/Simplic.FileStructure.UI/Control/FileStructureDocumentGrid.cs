using Simplic.Framework.DBUI;
using Simplic.Framework.Extension;
using System;
using System.Linq;
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

            // Handle additional script parameter
            integratedGridView.MenuHandler.RequestScriptParameter += OnRequestScriptParameter;

            integratedGridView.LoadConfiguration("Grid_Document_FileStructure");

            // Profile changed
            integratedGridView.SelectedProfileChanged += (s, e) =>
            {
                lastDirectoryId = DirectoryId;
                integratedGridView.EmbeddedGridView?.SetPlaceholder("[DirectoryId]", DirectoryId.ToString());
                integratedGridView.EmbeddedGridView?.SetPlaceholder("[FileStructureId]", FileStructureId.ToString());

                integratedGridView.EmbeddedGridView.SelectionChanged += (sender, args) =>
                {
                    if (args.AddedItems.Count > 0)
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        var blob = ArchivManager.Singleton.GetBlobByObjectDictionary("STACK_Document", integratedGridView.EmbeddedGridView.GetItemAsDictionary(args.AddedItems.First()));
#pragma warning restore CS0618 // Type or member is obsolete

                        if (blob != null) 
                        {
                            var blobId = (Guid)integratedGridView.EmbeddedGridView.SelectedItemAsDictionary["BlobGuid"];
                            Framework.Extension.UI.ViewerHelper.ShowDocument(blob, integratedGridView.EmbeddedGridView.SelectedItemAsDictionary, blobId, "default");
                        }
                    }
                };
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

        #region [Additional stack parameter]
        /// <summary>
        /// Generate dynamic doccenter/stack parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRequestScriptParameter(object sender, RequestScriptParameterEventArgs e)
        {
            var documentStackGuid = Guid.Parse("12C9B95B-BD33-4FA0-9CA1-05E11122018C");
            var parameter = new DocCenterParameter("STACK_Document", documentStackGuid);
                        
            e.AdditionalParameter = new object[] 
            {
                parameter
            };
            return;
        }
        #endregion

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
