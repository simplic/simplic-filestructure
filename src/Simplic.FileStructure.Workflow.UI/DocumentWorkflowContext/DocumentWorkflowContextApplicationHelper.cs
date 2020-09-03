using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Framework.DBUI;

namespace Simplic.FileStructure.Workflow.UI
{
    public class DocumentWorkflowContextApplicationHelper : GridWindowApplicationHelper<Guid, DocumentWorkflowContext, DocumentWorkflowContextViewModel>
    {
        public override string PrimaryKeyColumn => "Guid";

        public override Type WindowInterface => typeof(IDocumentWorkflowContextWindow);
    }
}
