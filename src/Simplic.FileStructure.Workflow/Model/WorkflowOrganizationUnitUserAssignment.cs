﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the assignment between the user and <see cref="WorkflowOrganizationUnitAssignment"/>
    /// </summary>
    public class WorkflowOrganizationUnitUserAssignment
    {
        /// <summary>
        /// Gets or sets the guid
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the workflow organization assignment id 
        /// <see cref="WorkflowOrganizationUnitAssignment"/>
        /// </summary>
        public Guid WorkflowOrganzitionAssignmentId{ get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int UserId { get; set; }
    }
}
