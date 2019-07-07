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
        private List<DirectoryClassification> availableDirectoryClassifications;
        private List<DirectoryClassification> chosenFieldTypes;
        private List<DirectoryTypeClassification> directoryTypeClassifications;
        private readonly IDirectoryClassificationService directoryClassificationService;
        private readonly IDirectoryTypeClassificationService directoryTypeClassificationService;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        public DirectoryTypeEditorViewModel()
        {
            directoryClassificationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationService>();
            directoryTypeClassificationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeClassificationService>();
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
        /// Gets or sets the type category
        /// </summary>
        public string Category
        {
            get
            {
                return model.Category;
            }
            set
            {
                PropertySetter(value, (newValue) => { model.Category = newValue; });
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

        /// <summary>
        /// Gets or sets the available directory classification pool
        /// </summary>
        public List<DirectoryClassification> AvailableDirectoryClassifications
        {
            get
            {
                if (availableDirectoryClassifications == null)
                    availableDirectoryClassifications = directoryClassificationService.GetAll().Where(f => !ChosenDirectoryClassifications.Any(c => c.Id == f.Id)).ToList();

                return availableDirectoryClassifications;
            }
            set
            {
                PropertySetter(value, (newValue) => { availableDirectoryClassifications = newValue; });
                RaisePropertyChanged("AvailableDirectoryClassifications");
            }
        }
        /// <summary>
        /// Gets or sets the chosen directory classifications
        /// </summary>
        public List<DirectoryClassification> ChosenDirectoryClassifications
        {
            get
            {
                if (chosenFieldTypes == null)
                {
                    chosenFieldTypes = new List<DirectoryClassification>();
                    foreach (var field in DirectoryTypeClassifications)
                        chosenFieldTypes.Add(directoryClassificationService.Get(field.DirectoryClassificationId));
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
        /// Gets the directory type classifications from Database
        /// </summary>
        public List<DirectoryTypeClassification> DirectoryTypeClassifications
        {
            get
            {
                if (directoryTypeClassifications == null)
                    directoryTypeClassifications = directoryTypeClassificationService.GetByDirectoryTypeId(Model.Id).ToList();

                return directoryTypeClassifications;
            }
            set
            {
                PropertySetter(value, (newValue) => { directoryTypeClassifications = newValue; });
            }
        }

    }
}
