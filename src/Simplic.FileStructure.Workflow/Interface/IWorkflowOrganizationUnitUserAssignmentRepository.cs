using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the interface for the repository of <see cref="WorkflowOrganizationUnitUserAssignment"/>
    /// </summary>
    public interface IWorkflowOrganizationUnitUserAssignmentRepository : IRepositoryBase<Guid, WorkflowOrganizationUnitUserAssignment>
    {
        /// <summary>
        /// Gets all <see cref="WorkflowOrganizationUnitUserAssignment"/> by a guid
        /// </summary>
        /// <param name="guid">The guid that is passed through</param>
        /// <returns>A list of <see cref="WorkflowOrganizationUnitUserAssignment"/></returns>
        IEnumerable<WorkflowOrganizationUnitUserAssignment> GetByAssignmentId(Guid guid);

        /// <summary>
        /// Gets all user assignments for a specific workflow and organization
        /// </summary>
        /// <param name="workflowConfigurationId">Unique workflow id</param>
        /// <param name="organizationUnitId">Unique organization id</param>
        /// <returns>List of user-assignments</returns>
        IEnumerable<WorkflowOrganizationUnitUserAssignment> GetForOrganizationByConfigurationId(Guid workflowConfigurationId, Guid organizationUnitId);
    }
}
