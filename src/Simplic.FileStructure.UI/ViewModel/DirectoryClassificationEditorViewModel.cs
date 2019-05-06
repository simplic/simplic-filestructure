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
    /// Directory classification editor viewmodel
    /// </summary>
    public class DirectoryClassificationEditorViewModel : ExtendableViewModel, IWindowViewModel<DirectoryClassification>
    {
        private DirectoryClassification model;
        private List<FieldType> availableFieldTypes;
        private List<FieldType> chosenFieldTypes;
        private List<DirectoryClassificationField> directoryClassificationFields;
        private readonly IFieldTypeService fieldTypeService;
        private readonly IDirectoryClassificationFieldService directoryClassificationFieldService;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        public DirectoryClassificationEditorViewModel()
        {
            fieldTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFieldTypeService>();
            directoryClassificationFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationFieldService>();
        }

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model instance</param>
        public void Initialize(DirectoryClassification model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets the model instance
        /// </summary>
        public DirectoryClassification Model
        {
            get
            {
                return model;
            }
        }

        /// <summary>
        /// Gets or sets the Name
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
        /// Gets or sets the available field type pool
        /// </summary>
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

        /// <summary>
        /// Gets or sets the chosen field types
        /// </summary>
        public List<FieldType> ChosenFieldTypes
        {
            get
            {
                if (chosenFieldTypes == null)
                {
                    chosenFieldTypes = new List<FieldType>();
                    foreach (var field in DirectoryClassificationFields)
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

        /// <summary>
        /// Gets the directory classification fields from Database
        /// </summary>
        public List<DirectoryClassificationField> DirectoryClassificationFields
        {
            get
            {
                if (directoryClassificationFields == null)
                    directoryClassificationFields = directoryClassificationFieldService.GetByDirectoryClassificationId(Model.Id).ToList();

                return directoryClassificationFields;
            }
        }

    }
}
