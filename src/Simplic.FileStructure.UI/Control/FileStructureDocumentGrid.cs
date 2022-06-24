using Simplic.Framework.DBUI;
using Simplic.Framework.Extension;
using Simplic.Framework.Extension.UI;
using Simplic.Session;
using System;
using System.Collections.Generic;
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
        private InstanceDataGrid integratedGridView;
        private Directory lastDirectory;
        private DirectoryType lastDirectoryType;
        private IDictionary<Guid, DirectoryType> typeCache = new Dictionary<Guid, DirectoryType>();
        private IList<InstanceDataGrid> grids = new List<InstanceDataGrid>();

        /// <summary>
        /// Directory id binding property
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register("Directory", typeof(Directory), typeof(FileStructureDocumentGrid), new PropertyMetadata(null, DirectoryIdChangedCallback));

        /// <summary>
        /// File structure id property
        /// </summary>
        public static readonly DependencyProperty FileStructureIdProperty =
            DependencyProperty.Register("FileStructureId", typeof(Guid), typeof(FileStructureDocumentGrid), new PropertyMetadata(Guid.Empty));

        private DirectoryType GetOrCreateDirectoryType(Guid typeId)
        {
            if (typeCache.ContainsKey(typeId))
                return typeCache[typeId];

            var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            var type = service.Get(typeId);

            typeCache[typeId] = type;

            return type;
        }

        private void SetGrid(string configurationName)
        {
            var sessionService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ISessionService>();
            integratedGridView = grids.FirstOrDefault(x => x.GridView.Configuration.Name == configurationName);

            if (integratedGridView != null)
                Content = integratedGridView;

            if (integratedGridView == null)
            {
                if (string.IsNullOrWhiteSpace(configurationName))
                    configurationName = "Grid_Document_FileStructure";

                integratedGridView = new InstanceDataGrid();
                integratedGridView.SetBlobSettings(true, true);
                integratedGridView.FillOnProfileChanged = false;
                Content = integratedGridView;

                // Handle additional script parameter
                integratedGridView.GridView.MenuHandler.RequestScriptParameter += OnRequestScriptParameter;

                // Profile changed
                integratedGridView.GridView.SelectedProfileChanged += (s, e) =>
                {
                    lastDirectory = Directory;
                    integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[DirectoryId]", Directory?.Id.ToString());
                    integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[FileStructureId]", FileStructureId.ToString());
                    integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[WorkflowId]", Directory?.WorkflowId.ToString());
                    integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[TenantId]", sessionService.CurrentSession.Organizations[0].Id.ToString());

                    integratedGridView.GridView.RefreshData();
                };
                integratedGridView.SetConfig(configurationName, String.Empty, StackHelper.Singleton.GetStackGuidByName("STACK_Document"), Guid.Empty, new List<Guid>());

                grids.Add(integratedGridView);
            }
        }

        /// <summary>
        /// Directory id changed callback
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DirectoryIdChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sessionService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ISessionService>();

            var grid = d as FileStructureDocumentGrid;
            if (grid.isLoaded)
            {
                if (e.NewValue == null)
                    return;

                var currentDirectory = (Directory)e.NewValue;
                var currentDirectoryType = grid.GetOrCreateDirectoryType(currentDirectory.DirectoryTypeId);

                if (grid.lastDirectoryType?.GridName != currentDirectoryType.GridName)
                    grid.SetGrid(currentDirectoryType.GridName);

                if (grid.lastDirectory != null && grid.lastDirectory != currentDirectory && currentDirectory != null)
                {
                    grid.integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[DirectoryId]", currentDirectory.Id.ToString());
                    grid.integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[FileStructureId]", grid.FileStructureId.ToString());
                    grid.integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[WorkflowId]", currentDirectory.WorkflowId.ToString());
                    grid.integratedGridView.GridView.EmbeddedGridView?.SetPlaceholder("[TenantId]", sessionService.CurrentSession.Organizations[0].Id.ToString());

                    grid.integratedGridView.GridView.RefreshData();

                    grid.lastDirectory = currentDirectory;
                    grid.lastDirectoryType = currentDirectoryType;
                }
                if (currentDirectory == null || currentDirectory.Id == Guid.Empty)
                {
                    grid.integratedGridView.GridView.CancelLoading();
                    grid.integratedGridView.GridView.EmbeddedGridView.Clear();

                    grid.lastDirectory = currentDirectory;
                    grid.lastDirectoryType = currentDirectoryType;
                }
            }
        }

        private bool isLoaded;

        /// <summary>
        /// Initialize grid
        /// </summary>
        public FileStructureDocumentGrid()
        {
            SetGrid("Grid_Document_FileStructure");

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
        public Directory Directory
        {
            get { return (Directory)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
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
