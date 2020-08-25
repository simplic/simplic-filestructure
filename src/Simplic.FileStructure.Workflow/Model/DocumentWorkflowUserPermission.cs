using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the permission a user has on a specific workflow 
    /// </summary>
    public class DocumentWorkflowUserPermission
    {
        /// <summary>
        /// Gets or sets the guid thats represent the primary key in the database
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the workflow id 
        /// </summary>
        public Guid WorkflowId { get; set; }
        
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the permission to set the permission if said user can edit the workflow
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the permission to set the permission if said user can read the workflow
        /// </summary>
        public bool CanRead { get; set; }


    }
}
