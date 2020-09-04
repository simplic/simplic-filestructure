using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Framework.DBUI;

namespace Simplic.FileStructure.Workflow.UI
{
    public class DocumentWorkflowConfigurationApplicationHelper : GridWindowApplicationHelper<Guid, DocumentWorkflowConfiguration, DocumentWorkflowConfigurationViewModel>
    {
        public override string PrimaryKeyColumn => "Guid";

        public override Type WindowInterface => typeof(IDocumentWorkflowConfigurationWindow);
    }
}
