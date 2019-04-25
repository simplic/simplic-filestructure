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
    public class FieldTypeEditorViewModel : ExtendableViewModel, IWindowViewModel<FieldType>
    {
        private FieldType model;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        public FieldTypeEditorViewModel()
        {

        }

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model instance</param>
        public void Initialize(FieldType model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets the model instance
        /// </summary>
        public FieldType Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string Name
        {
            get
            {
                return model.Name;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.Name = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string Datatype
        {
            get
            {
                return model.Datatype;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.Datatype = newValue; });
            }
        }
    }
}
