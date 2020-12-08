using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class WorkflowOperation
    {
        /// <summary>
        /// Gets or sets the guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the workflow id 
        /// </summary>
        public Guid WorkflowId { get; set; }

        /// <summary>
        /// Gets or sets the user id 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the target user id
        /// </summary>
        public int TargetUserId { get; set; }

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the document id, which is based on the poco document
        /// </summary>
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// Gets or sets the document path
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

        /// <summary>
        /// Gets or sets the operation type, the default value is <see cref="WorkflowOperationType.User"/>
        /// </summary>
        public WorkflowOperationType OperationType { get; set; } = WorkflowOperationType.User;

        /// <summary>
        /// Gets or sets the workflow organization id, its based on the guid of <see cref="WorkflowOrganizationUnit.Guid"/>
        /// </summary>
        public Guid? WorkflowOrganzisationId { get; set; }

        /// <summary>
        /// Gets or sets the directory id
        /// </summary>
        public Guid? DirectoryId { get; set; }

        /// <summary>
        /// Gets or sets the file structure id
        /// </summary>
        public Guid? FileStructureId { get; set; }
    }
}
