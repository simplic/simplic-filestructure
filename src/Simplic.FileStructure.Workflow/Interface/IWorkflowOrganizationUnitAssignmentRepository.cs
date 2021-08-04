using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;
using Simplic.FileStructure.Workflow;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines methods for storing the assignments between  <see cref="DocumentWorkflowConfiguration"/> and <see cref="WorkflowOrganizationUnit"/>
    /// </summary>
    public interface IWorkflowOrganizationUnitAssignmentRepository : IRepositoryBase<Guid, WorkflowOrganizationUnitAssignment>
    {
        /// <summary>
        /// Gets a list of <see cref="WorkflowOrganizationUnitAssignment"/> by a workflow id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        IEnumerable<WorkflowOrganizationUnitAssignment> GetByWorkflowId(Guid guid);
    }
}
