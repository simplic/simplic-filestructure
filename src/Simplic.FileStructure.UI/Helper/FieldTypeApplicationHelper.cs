﻿using Simplic.Framework.DBUI;
using System;

namespace Simplic.FileStructure.UI.Helper
{
    /// <summary>
    /// Field Type Application helper
    /// </summary>
    public class FieldTypeApplicationHelper : GridWindowApplicationHelper<Guid, FieldType, FieldTypeEditorViewModel>
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
                return typeof(IFieldTypeEditor);
            }
        }
    }
}
