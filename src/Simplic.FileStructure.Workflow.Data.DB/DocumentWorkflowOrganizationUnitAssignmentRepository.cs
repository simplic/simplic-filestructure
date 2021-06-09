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

        public bool DeleteByIds(Guid documentId, Guid organizationId)
        {
            sqlService.OpenConnection((connection) =>
            {
                connection.Execute($"Delete from {TableName} where DocumentId =:documentId and WorkflowOrganizationUnitId = :organizationId",
                    new { documentId, organizationId });
            });
            return true;
        }

        public DocumentWorkflowOrganizationUnitAssignment GetByIds(Guid documentId, Guid organizationId)
        {
            return sqlService.OpenConnection((connection) =>
            {
               return connection.QueryFirstOrDefault<DocumentWorkflowOrganizationUnitAssignment>($"Select * from {TableName} where DocumentId =:documentId and WorkflowOrganizationUnitId = :organizationId",
                    new { documentId, organizationId });
            });
        }

        public override Guid GetId(DocumentWorkflowOrganizationUnitAssignment obj) => obj.Guid;
    }
}
