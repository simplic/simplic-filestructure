using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the workflow operation
    /// </summary>
    public class WorkflowOperation
    {
        /// <summary>
        /// Gets or sets the guid 
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or set the internal workflow name
        /// </summary>
        public string InternalWorkflowName { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public int TargetUserId { get; set; }

        /// <summary>
        /// Gets or sets the user name 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// Gets or sets the document path guid
        /// </summary>
        public Guid DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the create date time 
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        
        /// <summary>
        /// Gets or sets the update date time 
        /// </summary>
        public DateTime UpdateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the action name 
        /// </summary>
        public string ActionName { get; set; }


    }
}
