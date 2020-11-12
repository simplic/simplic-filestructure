using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Simplic.Collections.Generic;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Document workflow context to create workflows
    /// </summary>
    public class DocumentWorkflowConfiguration
    {
        /// <summary>
        /// Gets or sets the guid
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the internal name
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the state provider
        /// </summary>
        public string StateProviderName { get; set; }

        public StatefulCollection<WorkflowOrganizationUnitAssignment> OrganizationUnits { get; set; } = new StatefulCollection<WorkflowOrganizationUnitAssignment>(new WorkflowOrganizationUnitAssignment[] { });
    }
}
