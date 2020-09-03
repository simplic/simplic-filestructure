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
    /// Interaction logic for DocumentWorkflowContextWindow.xaml
    /// </summary>
    public partial class DocumentWorkflowContextWindow : BaseDocumentWorkflowContextWindow
    {
        public DocumentWorkflowContextWindow(IDocumentWorkflowContextService service) : base(service)
        {
            InitializeComponent();
        }
    }
    public abstract class BaseDocumentWorkflowContextWindow : ApplicationWindow<Guid, DocumentWorkflowContext, DocumentWorkflowContextViewModel, IDocumentWorkflowContextService>, IDocumentWorkflowContextWindow
    {
        public BaseDocumentWorkflowContextWindow(IDocumentWorkflowContextService service)
            : base(service)
        {
        }
    }
}
