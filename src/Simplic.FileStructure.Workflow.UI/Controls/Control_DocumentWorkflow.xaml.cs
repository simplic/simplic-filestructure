using Simplic.FileStructure.Workflow;
using Simplic.Framework.UI.Info;
using Simplic.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simplic.FileStructure.Workflow.UI.Controls
{
    /// <summary>
    /// Interaction logic for Control_DocumentWorkflow.xaml
    /// </summary>
    public partial class Control_DocumentWorkflow : Simplic.Framework.UI.Page
    {
        #region Private Member        
        private readonly Guid stackGuid = Guid.Parse("0336830B-5689-4E28-9900-F9BC2197F13B");

        private readonly IDocumentWorkflowUserService documentWorkflowUserService;
        private readonly IDocumentWorkflowAppSettingsService documentWorkflowAppSettingsService;
        private readonly ISessionService sessionService;
        private readonly IFileStructureService fileStructureService;
        private readonly DocumentWorkflowAppSettings documentWorkflowAppSettings;
        private DocumentWorkflowUser documentWorkflowUser;
        private FileStructure fileStructureConfiguration;
        #endregion

        internal Control_DocumentWorkflow(DBInternPage DBInternPage)
        {
            InitializeComponent();

            documentWorkflowUserService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentWorkflowUserService>();
            documentWorkflowAppSettingsService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentWorkflowAppSettingsService>();
            fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            sessionService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ISessionService>();
            documentWorkflowAppSettings = documentWorkflowAppSettingsService.Get(DBInternPage.Guid);

            if (documentWorkflowAppSettings == null || string.IsNullOrWhiteSpace(documentWorkflowAppSettings.InternalName))
            {
                MessageBox.Show("Konfiguration nicht vorhanden", "...");
                return;
            }

            PageName = documentWorkflowAppSettings.PublicName;

            documentWorkflowUser = documentWorkflowUserService.Get(sessionService.CurrentSession.UserId);

            if (documentWorkflowUser == null)
            {
                documentWorkflowUser = new DocumentWorkflowUser
                {
                    UserId = sessionService.CurrentSession.UserId,
                    IsDeleted = false
                    // TODO: Set current tenant
                };

                documentWorkflowUserService.Save(documentWorkflowUser);
            }

            fileStructureConfiguration = fileStructureService.GetByInstanceDataGuid(documentWorkflowUser.Guid);
            if (fileStructureConfiguration == null)
            {
                fileStructureConfiguration = new FileStructure
                {
                    IsTemplate = false,
                    InstanceDataGuid = documentWorkflowUser.Guid,
                    Id = Guid.NewGuid(),
                    Name = documentWorkflowAppSettings.PublicName,
                    StackGuid = stackGuid
                };

                fileStructureService.Save(fileStructureConfiguration);
            }

            fileStructureConfiguration.Name = $"{documentWorkflowAppSettings.PublicName}({Framework.Base.UserManager.Singleton.GetFriendlyName(sessionService.CurrentSession.UserId)})";

            fileStructureControl.Initialize(fileStructureConfiguration);

            Loaded += ControlLoaded;
        }

        #region Private Methods
        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            LayoutDocumentParent.Closed += LayoutDocumentParentClosed;

            Loaded -= ControlLoaded;
        }

        private void LayoutDocumentParentClosed(object sender, EventArgs e)
        {
            fileStructureService.Save(fileStructureConfiguration);

            LayoutDocumentParent.Closed += LayoutDocumentParentClosed;
        }

        private void SaveSettingButton(object sender, RoutedEventArgs e)
        {
            fileStructureService.Save(fileStructureConfiguration);
        }
        #endregion
    }
}
