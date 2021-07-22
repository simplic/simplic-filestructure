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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="documentId"><inheritdoc/></param>
        /// <param name="userId"><inheritdoc/></param>
        /// <returns></returns>
        public IEnumerable<DocumentWorkflowOrganizationUnitAssignment> GetByIds(Guid documentId, long userId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                string sql = $"SELECT docas.*, wouser.UserId FROM IT_Document_WorkflowOrganizationUnit_Assignment docas" +
                $" join FileStructure_WorkflowOrganizationUnit_Assignment wouas on docas.WorkflowOrganizationUnitId = wouas.WorkflowOrganisationUnitId and wouas.WorkflowId = docas.WorkflowId " +
                $"join FileStructure_WorkflowOrganizationUnit_UserAssignment wouser on wouser.WorkflowOrganzitionAssignmentId = wouas.Guid " +
                    $"Where 1 = 1 " +
                    $"And wouser.UserId = :userId " +
                    $"AND docas.DocumentId = :documentId ";
                return connection.Query<DocumentWorkflowOrganizationUnitAssignment>(sql,
                    new { documentId = documentId, userId = userId});
            });
        }

        public override Guid GetId(DocumentWorkflowOrganizationUnitAssignment obj) => obj.Guid;
    }
}
