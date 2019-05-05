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
    public class DirectoryTypeEditorViewModel : ExtendableViewModel, IWindowViewModel<DirectoryType>
    {
        private DirectoryType model;
        private List<FieldType> availableFieldTypes;
        private List<FieldType> chosenFieldTypes;
        private List<DirectoryTypeField> directoryTypeFields;
        private readonly IFieldTypeService fieldTypeService;
        private readonly IDirectoryTypeFieldService directoryTypeFieldService;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        public DirectoryTypeEditorViewModel()
        {
            fieldTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFieldTypeService>();
            directoryTypeFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeFieldService>();
        }

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model instance</param>
        public void Initialize(DirectoryType model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets the model instance
        /// </summary>
        public DirectoryType Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets the icon name
        /// </summary>
        public Guid IconId
        {
            get
            {
                return model.IconId;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.IconId = newValue; });
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
        /// Gets or sets whether drag/drop is enabled
        /// </summary>
        public bool EnableDrag
        {
            get
            {
                return model.EnableDrag;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.EnableDrag = newValue; });
            }
        }

        /// <summary>
        /// Gets or sets whether dropping is enabled
        /// </summary>
        public bool EnableDrop
        {
            get
            {
                return model.EnableDrop;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.EnableDrop = newValue; });
            }
        }

        public List<FieldType> AvailableFieldTypes
        {
            get
            {
                if (availableFieldTypes == null)
                    availableFieldTypes = fieldTypeService.GetAll().Where(f => !ChosenFieldTypes.Any(c => c.Id == f.Id)).ToList();

                return availableFieldTypes;
            }
            set
            {
                PropertySetter(value, (newValue) => { availableFieldTypes = newValue; });
                RaisePropertyChanged("AvailableFieldTypes");
            }
        }

        public List<FieldType> ChosenFieldTypes
        {
            get
            {
                if (chosenFieldTypes == null)
                {
                    chosenFieldTypes = new List<FieldType>();
                    foreach (var field in DirectoryTypeFields)
                        chosenFieldTypes.Add(fieldTypeService.Get(field.FieldTypeId));
                }

                return chosenFieldTypes;
            }
            set
            {
                PropertySetter(value, (newValue) => { chosenFieldTypes = newValue; });
                RaisePropertyChanged("ChosenFieldTypes");
            }
        }

        public List<DirectoryTypeField> DirectoryTypeFields
        {
            get
            {
                if (directoryTypeFields == null)
                    directoryTypeFields = directoryTypeFieldService.GetByDirectoryTypeId(Model.Id.ToString()).ToList();

                return directoryTypeFields;
            }
            set
            {
                PropertySetter(value, (newValue) => { directoryTypeFields = newValue; });
            }
        }

    }
}
