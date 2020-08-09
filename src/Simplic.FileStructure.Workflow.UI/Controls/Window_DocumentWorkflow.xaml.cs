using Simplic.FileStructure.Workflow;
using Simplic.FileStructure.Workflow.UI.ViewModel;
using Simplic.Framework.UI;
using Simplic.Framework.UI.Info;
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
    public abstract class BaseDocumentWorkflowAppSettingsWindow : ApplicationWindow<Guid, DocumentWorkflowAppSettings, DocumentWorkflowAppSettingsViewModel, IDocumentWorkflowAppSettingsService>, IDocumentWorkflowAppSettingsWindow
    {
        public BaseDocumentWorkflowAppSettingsWindow(IDocumentWorkflowAppSettingsService service)
            : base(service)
        {

        }

        /// <summary>
        /// Gets or sets the actual db intern page instance
        /// </summary>
        public virtual DBInternPage DBInternPage { get; set; }
    }
    /// <summary>
    /// Interaction logic for Window_DocumentWorkflow.xaml
    /// </summary>
    public partial class Window_DocumentWorkflowAppSettings : BaseDocumentWorkflowAppSettingsWindow
    {
        private readonly IDocumentWorkflowAppSettingsService service;

        public Window_DocumentWorkflowAppSettings(IDocumentWorkflowAppSettingsService service) : base(service)
        {
            InitializeComponent();
            this.service = service;
        }

        public override void OnSave(WindowSaveEventArg e)
        {
            if (DataContext is DocumentWorkflowAppSettingsViewModel model)
                model.Model.Guid = base.DBInternPage.Guid;

            base.OnSave(e);
        }
    }
}
