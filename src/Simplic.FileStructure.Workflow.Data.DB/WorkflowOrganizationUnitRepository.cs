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
    public class WorkflowOrganizationUnitRepository : SqlRepositoryBase<Guid, WorkflowOrganizationUnit>, IWorkflowOrganizationUnitRepository
    {
        private readonly ISqlService sqlService;

        public WorkflowOrganizationUnitRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }


        public override string TableName => "FileStructure_WorkflowOrganizationUnit";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(WorkflowOrganizationUnit obj) => obj.Guid;

    }
   
}
