using Simplic.Framework.DBUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI.Helper
{
    /// <summary>
    /// Grid application helper
    /// </summary>
    public class ApplicationHelper
    {
        /// <summary>
        /// Open file structure editor for instance data
        /// </summary>
        /// <param name="parameter">Grid function parameter</param>
        /// <returns>Refresh mode</returns>
        public static GridInvokeMethodResult OpenFileStructureEditor(GridFunctionParameter parameter)
        {
            var fileStructureWindow = new FileStructureWindow();
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
    }
}
