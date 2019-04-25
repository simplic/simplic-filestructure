using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Directory type editor viewmodel
    /// </summary>
    public class DirectoryTypeFieldEditorViewModel : ExtendableViewModel, IWindowViewModel<DirectoryTypeField>
    {
        private DirectoryTypeField model;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        public DirectoryTypeFieldEditorViewModel()
        {

        }

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model instance</param>
        public void Initialize(DirectoryTypeField model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets the model instance
        /// </summary>
        public DirectoryTypeField Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets the type DirectoryTypeId
        /// </summary>
        public Guid DirectoryTypeId
        {
            get
            {
                return model.DirectoryTypeId;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.DirectoryTypeId = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets the type FieldTypeId
        /// </summary>
        public Guid FieldTypeId
        {
            get
            {
                return model.FieldTypeId;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.FieldTypeId = newValue; });
            }
        }

    }
}
