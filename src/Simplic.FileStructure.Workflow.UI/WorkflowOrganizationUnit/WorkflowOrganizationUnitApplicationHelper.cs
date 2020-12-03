using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.FileStructure.Workflow.UI;
using Simplic.Framework.DBUI;

namespace Simplic.FileStructure.Workflow.UI
{

    public class WorkflowOrganizationUnitApplicationHelper : GridWindowApplicationHelper<Guid, WorkflowOrganizationUnit, WorkflowOrganizationUnitViewModel>
    {
        public override string PrimaryKeyColumn => "Guid";

        public override Type WindowInterface => typeof(IWorkflowOrganizationUnitWindow);

    }
}
