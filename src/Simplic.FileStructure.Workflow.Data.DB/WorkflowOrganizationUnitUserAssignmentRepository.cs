using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override string TableName => "FileStructure_WorkflowOrganizationUnit_UserAssignment";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(WorkflowOrganizationUnitUserAssignment obj) => obj.Guid;
    }
}
