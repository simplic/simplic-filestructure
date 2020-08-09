using Simplic.FileStructure.Workflow.UI.PageGoverner;
using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.UI
{
    public static class DocumentWorkflowInit
    {
        public static void Initialize()
        {
            PageManager.Singleton.AddPage(Guid.Parse("09456C33-53C8-41F7-A067-3EEF0CF26742"), new DocumentWorkflowPageGovernor());
        }
    }
}
