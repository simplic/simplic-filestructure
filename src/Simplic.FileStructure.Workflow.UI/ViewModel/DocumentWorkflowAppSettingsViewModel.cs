using Simplic.FileStructure.Workflow;
using Simplic.Framework.UI;
using Simplic.Localization;
using Simplic.Studio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.UI.ViewModel
{
    public class DocumentWorkflowAppSettingsViewModel : ExtendableViewModel, IWindowViewModel<DocumentWorkflowAppSettings>
    {
        private DocumentWorkflowAppSettings model;
        private ILocalizationService localizationSerivce;
        public DocumentWorkflowAppSettingsViewModel()
        {
            localizationSerivce = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
        }

        public string Title
        {
            get
            {
                if (localizationSerivce == null)
                {
                    return "Titel";
                }
                return localizationSerivce.Translate("document_workflow_title");
            }

        }

        public string InternalName
        {
            get => model.InternalName;
            set => model.InternalName = value;
        }

        public string PublicName
        {
            get => model.PublicName;
            set => model.PublicName = value;
        }

        public string InternalNameKey
        {
            get
            {
                if (localizationSerivce == null)
                {
                    return "Interner Name";
                }
                return localizationSerivce.Translate("document_workflow_internal_name");
            }

        }

        public string PublicNameKey
        {
            get
            {
                if (localizationSerivce == null)
                {
                    return "Öffentlicher Name";
                }
                return localizationSerivce.Translate("document_workflow_public_name");
            }
            set
            {

            }
        }

        public DocumentWorkflowAppSettings Model { get => model; }

        public void Initialize(DocumentWorkflowAppSettings model)
        {
            this.model = model;
        }
    }
}
