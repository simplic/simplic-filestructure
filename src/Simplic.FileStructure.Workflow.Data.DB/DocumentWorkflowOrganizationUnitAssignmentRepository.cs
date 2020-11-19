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
    public class DocumentWorkflowOrganizationUnitAssignmentRepository : SqlRepositoryBase<Guid, DocumentWorkflowOrganizationUnitAssignment>, IDocumentWorkflowOrganizationUnitAssignmentRepository
    {
        private ISqlService sqlService;
        public DocumentWorkflowOrganizationUnitAssignmentRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        public override string TableName => "IT_Document_WorkflowOrganizationUnit_Assignment";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowOrganizationUnitAssignment obj) => obj.Guid;
    }
}
