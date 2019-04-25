using Simplic.Framework.DBUI;
using System;

namespace Simplic.FileStructure.UI.Helper
{
    /// <summary>
    /// Application helper
    /// </summary>
    public class DirectoryTypeFieldApplicationHelper : GridWindowApplicationHelper<Guid, DirectoryTypeField, DirectoryTypeFieldEditorViewModel>
    {
        /// <summary>
        /// Gets the priamry column (Id)
        /// </summary>
        public override string PrimaryKeyColumn
        {
            get
            {
                return "Id";
            }
        }

        /// <summary>
        /// Window interface
        /// </summary>
        public override Type WindowInterface
        {
            get
            {
                return typeof(IDirectoryTypeFieldEditor);
            }
        }
    }
}
