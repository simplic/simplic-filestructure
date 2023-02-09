using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents a document workflow tracker.
    /// </summary>
    public class DocumentWorkflowTracker
    {
        /// <summary>
        /// Gets or sets a guid that represents a primary key.
        /// Default value is a new GUID.
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets a document id.
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Gets or sets a user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets a create date time.
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets a target user id.
        /// </summary>
        public int? TargetUserId { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public DocumentWorkflowStateType ActionName { get; set; }

        /// <summary>
        /// Gets or sets a nullable workflow organization id.
        /// </summary>
        public Guid? WorkflowOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }
    }
}
