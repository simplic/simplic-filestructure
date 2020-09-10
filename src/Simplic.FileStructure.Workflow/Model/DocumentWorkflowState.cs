using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class DocumentWorkflowState
    {
        /// <summary>
        /// Gets or sets the guid
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the internal name
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public DocumentWorkflowStateType StateType { get; set; }
    }
}
