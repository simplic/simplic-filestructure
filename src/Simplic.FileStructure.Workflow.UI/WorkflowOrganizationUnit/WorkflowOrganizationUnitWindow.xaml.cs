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
using Simplic.Framework.UI;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// Interaction logic for WorkflowOrganizationUnitWindow.xaml
    /// </summary>
    
    public partial class WorkflowOrganizationUnitWindow : BaseWorkflowOrganizationUnitWindow
    {
        public WorkflowOrganizationUnitWindow(IWorkflowOrganizationUnitService service) : base(service)
        {
            
            InitializeComponent();
        }
    }
    public abstract class BaseWorkflowOrganizationUnitWindow : ApplicationWindow<Guid, WorkflowOrganizationUnit, WorkflowOrganizationUnitViewModel, IWorkflowOrganizationUnitService>, IWorkflowOrganizationUnitWindow
    {
        public BaseWorkflowOrganizationUnitWindow(IWorkflowOrganizationUnitService service)
            : base(service)
        {
        }
    }
}
