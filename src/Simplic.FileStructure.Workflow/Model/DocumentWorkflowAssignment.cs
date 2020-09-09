using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the document workflow assignment
    /// </summary>
    public class DocumentWorkflowAssignment
    {
        /// <summary>
        /// Gets or sets the guid which stands for the primary key 
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the workflow id 
        /// </summary>
        public Guid WorkflowId { get; set; }

        /// <summary>
        /// Gets or sets the state id
        /// </summary>
        public Guid StateId { get; set; }
    }
}
