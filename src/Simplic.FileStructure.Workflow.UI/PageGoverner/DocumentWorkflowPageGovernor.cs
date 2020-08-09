using Simplic.FileStructure.Workflow;
using Simplic.FileStructure.Workflow.UI.Controls;
using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.UI.PageGoverner
{
    public class DocumentWorkflowPageGovernor : PageGovernor
    {
        public override Page CreatePage(Framework.UI.Info.DBInternPage DBInternPage)
        {
            return new Control_DocumentWorkflow(DBInternPage);
        }

        public override System.Windows.Window EditPage(Framework.UI.Info.DBInternPage DBInternPage)
        {
            var window = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentWorkflowAppSettingsWindow>();
            var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentWorkflowAppSettingsService>();

            window.DBInternPage = DBInternPage;

            var data = service.Get(window.DBInternPage.Guid);
            if (data == null)
                window.ShowDialog(window.DBInternPage.Guid, Studio.UI.Mode.New);
            else
                window.ShowDialog(window.DBInternPage.Guid, Studio.UI.Mode.Edit);

            return null;
        }
    }
}
