using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the document workflow context 
    /// </summary>
    public class DocumentWorkflowContext
    {
        /// <summary>
        /// Gets or sets the primary key as guid
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the document id as guid
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the workflow id as guid
        /// </summary>
        public Guid WorkflowId { get; set; }

        public int Placeholder1 { get; set; }
        public int Placeholder2 { get; set; }
        public int Placeholder3 { get; set; }
        public int Placeholder4 { get; set; }
    }
}
