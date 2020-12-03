using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Framework.UI;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// Represents the view model to select the <see cref="WorkflowOrganizationUnit"/>
    /// </summary>
    public class SelectWorkflowOrganizationUnitViewModel : ExtendableViewModel
    {
        private readonly WorkflowOrganizationUnit workflowOrganizationUnit;

        /// <summary>
        /// Constructor to pass the <see cref="WorkflowOrganizationUnit"/>
        /// </summary>
        /// <param name="workflowOrganizationUnit"></param>
        public SelectWorkflowOrganizationUnitViewModel(WorkflowOrganizationUnit workflowOrganizationUnit)
        {
            this.workflowOrganizationUnit = workflowOrganizationUnit;
        }

        public string Name => workflowOrganizationUnit.DisplayName;

        /// <summary>
        /// Gets or sets the model
        /// </summary>
        public WorkflowOrganizationUnit Model => workflowOrganizationUnit;

        /// <summary>
        /// Gets or sets the Guid
        /// </summary>
        public Guid Guid => workflowOrganizationUnit.Guid;
    }
}
