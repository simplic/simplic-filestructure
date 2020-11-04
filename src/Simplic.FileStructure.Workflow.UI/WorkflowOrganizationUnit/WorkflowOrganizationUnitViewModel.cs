using Simplic.Framework.UI;
using Simplic.Studio.UI;

namespace Simplic.FileStructure.Workflow.UI
{
    public class WorkflowOrganizationUnitViewModel : ExtendableViewModel, IWindowViewModel<WorkflowOrganizationUnit>
    {
        public WorkflowOrganizationUnit Model { get; set; }

        public void Initialize(WorkflowOrganizationUnit model)
        {
            if (model == null)
            {
                model = new WorkflowOrganizationUnit();
            }
            Model = model;
        }
    }
}
