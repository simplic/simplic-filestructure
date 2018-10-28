using Simplic.Framework.DBUI;
using Simplic.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Simplic.FileStructure.UI.Helper
{
    /// <summary>
    /// Grid application helper
    /// </summary>
    public class ApplicationHelper
    {
        private static IFileStructureService fileStructureService;
        private static ILocalizationService localizationService;

        /// <summary>
        /// Initialize helper
        /// </summary>
        static ApplicationHelper()
        {
            fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
        }

        #region [NewFileStructureTemplate]
        /// <summary>
        /// New file structure template
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        /// <returns>Grid invoke result, to control grid refresh</returns>
        public static GridInvokeMethodResult NewFileStructureTemplate(GridFunctionParameter parameter)
        {
            var fileStructureWindow = new FileStructureWindow();

            // Create new, maybe template
            var fileStructure = new FileStructure
            {
                IsTemplate = true,
                Name = "Template",
                Directories = new List<Directory>
                {
                    new Directory
                    {
                        Id = Guid.NewGuid(),
                        Name = localizationService.Translate("filestructure_add_directory")
                    }
                }
            };

            fileStructureWindow.Initialize(fileStructure);
            fileStructureWindow.WindowMode = Framework.UI.WindowMode.Edit;
            fileStructureWindow.Show();

            // Refresh grid after closed
            fileStructureWindow.Closed += (s, e) =>
            {
                parameter.GridView.RefreshData();
            };

            return new GridInvokeMethodResult
            {
                RefreshGrid = false,
                Window = fileStructureWindow
            };
        }
        #endregion

        #region [EditFileStructure]
        /// <summary>
        /// Edit file structure
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        /// <returns>Grid invoke result, to control grid refresh</returns>
        public static GridInvokeMethodResult EditFileStructure(GridFunctionParameter parameter)
        {
            foreach (var id in parameter.GetSelectedRowsAsDataRow().Select(x => (Guid)x["Id"]))
            {
                var fileStructureWindow = new FileStructureWindow();

                // Get and copy
                var fileStructure = fileStructureService.Get(id);

                fileStructureWindow.Initialize(fileStructure);
                fileStructureWindow.WindowMode = Framework.UI.WindowMode.Edit;
                fileStructureWindow.Show();

                // Refresh grid after closed
                fileStructureWindow.Closed += (s, e) =>
                {
                    parameter.GridView.RefreshData();
                };

                return new GridInvokeMethodResult
                {
                    RefreshGrid = false,
                    Window = fileStructureWindow
                };
            }

            // Do not refresh grid if no data is passed
            return new GridInvokeMethodResult { RefreshGrid = false };
        }
        #endregion

        #region [CopyFileStructure]
        /// <summary>
        /// Copy file structure
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        /// <returns>Grid invoke result, to control grid refresh</returns>
        public static GridInvokeMethodResult CopyFileStructure(GridFunctionParameter parameter)
        {
            foreach (var id in parameter.GetSelectedRowsAsDataRow().Select(x => (Guid)x["Id"]))
            {
                var fileStructureWindow = new FileStructureWindow();

                // Get and copy
                var fileStructure = fileStructureService.Get(id);
                fileStructure = fileStructure.Copy();

                fileStructureWindow.Initialize(fileStructure);

                // Force save
                fileStructureWindow.ViewModel.IsDirty = true;

                fileStructureWindow.Show();

                // Refresh grid after closed
                fileStructureWindow.Closed += (s, e) =>
                {
                    parameter.GridView.RefreshData();
                };

                return new GridInvokeMethodResult
                {
                    RefreshGrid = false,
                    Window = fileStructureWindow
                };
            }

            // Do not refresh grid if no data is passed
            return new GridInvokeMethodResult { RefreshGrid = false };
        }
        #endregion

        #region [OpenFileStructureEditor]
        /// <summary>
        /// Open file structure editor for instance data
        /// </summary>
        /// <param name="parameter">Grid parameter</param>
        /// <returns>Grid invoke result, to control grid refresh</returns>
        public static GridInvokeMethodResult OpenFileStructureEditor(GridFunctionParameter parameter)
        {
            var fileStructureWindow = new FileStructureWindow();

            // TODO: Maybe change to foreach....
            var instanceDataGuid = parameter.GetSelectedRowsAsDataRow().Select(x => (Guid)x["Guid"]).FirstOrDefault();

            var fileStructure = fileStructureService.GetByInstanceDataGuid(instanceDataGuid);
            if (fileStructure == null)
            {
                var selectFromTemplateResult = MessageBox.Show("filestructure_select_template_msg", "filestructure_select_template_title", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (selectFromTemplateResult == MessageBoxResult.No)
                {
                    // Create new, maybe from template?
                    fileStructure = new FileStructure
                    {
                        InstanceDataGuid = instanceDataGuid,
                        IsTemplate = false
                    };
                }
                else
                {
                    var templateItemBox = ItemBoxManager.GetItemBoxFromDB("IB_FileStructureTemplate");
                    templateItemBox.ShowDialog();

                    if (templateItemBox.SelectedItem != null)
                    {
                        var templateId = (Guid)templateItemBox.GetSelectedItemCell("Id");
                        var template = fileStructureService.Get(templateId);

                        // Copy template and connect with instance data entry
                        fileStructure = template.Copy();
                        fileStructure.IsTemplate = false;
                        fileStructure.InstanceDataGuid = instanceDataGuid;
                    }
                }

                // Exit if no file structure is created
                if (fileStructure == null)
                    return new GridInvokeMethodResult { RefreshGrid = false };
            }

            // Initialize window
            fileStructureWindow.Initialize(fileStructure);

            // Force save
            fileStructureWindow.ViewModel.IsDirty = true;

            fileStructureWindow.Show();

            // Refresh grid after closed
            fileStructureWindow.Closed += (s, e) =>
            {
                parameter.GridView.RefreshData();
            };

            return new GridInvokeMethodResult
            {
                RefreshGrid = false,
                Window = fileStructureWindow
            };
        }
        #endregion
    }
}
