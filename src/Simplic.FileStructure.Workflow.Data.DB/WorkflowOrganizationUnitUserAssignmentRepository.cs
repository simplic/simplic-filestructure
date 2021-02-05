using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;

namespace Simplic.FileStructure.Workflow.Data.DB
{
    /// <summary>
    /// Represents the workflow organization user assignment repository
    /// </summary>
    public class WorkflowOrganizationUnitUserAssignmentRepository : SqlRepositoryBase<Guid, WorkflowOrganizationUnitUserAssignment>, IWorkflowOrganizationUnitUserAssignmentRepository
    {
        private readonly ISqlService sqlService;

        public WorkflowOrganizationUnitUserAssignmentRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        public IEnumerable<WorkflowOrganizationUnitUserAssignment> GetByAssignmentId(Guid guid) => GetAllByColumn("WorkflowOrganzitionAssignmentId", guid);

        /// <summary>
        /// Gets all user assignments for a specific workflow and organization
        /// </summary>
        /// <param name="workflowConfigurationId">Unique workflow id</param>
        /// <param name="organizationUnitId">Unique organization id</param>
        /// <returns>List of user-assignments</returns>
        public IEnumerable<WorkflowOrganizationUnitUserAssignment> GetForOrganizationByConfigurationId(Guid workflowConfigurationId, Guid organizationUnitId)
        {
            return sqlService.OpenConnection((c) => 
            {
                return c.Query<WorkflowOrganizationUnitUserAssignment>($@"
                SELECT ua.*
                FROM {TableName} ua
                JOIN FileStructure_WorkflowOrganizationUnit_Assignment a on a.Guid = ua.WorkflowOrganzitionAssignmentId
                WHERE a.WorkflowId = :{nameof(workflowConfigurationId)} and a.WorkflowOrganisationUnitId = :{nameof(organizationUnitId)}
                ", new { workflowConfigurationId, organizationUnitId });
            });
        }

        public override string TableName => "FileStructure_WorkflowOrganizationUnit_UserAssignment";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(WorkflowOrganizationUnitUserAssignment obj) => obj.Guid;
    }
}
