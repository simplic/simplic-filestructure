using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Simplic.Sql;

namespace Simplic.FileStructure.Workflow.Data.DB
{
    public class DocumentWorkflowStateProviderRepository : IDocumentWorkflowStateProviderRepository
    {
        private readonly ISqlService sqlService;

        public DocumentWorkflowStateProviderRepository(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }
        

        public bool IsDocumentInWorkflowCompleted(Guid documentId, Guid workflowId)
        {
            
                return sqlService.OpenConnection((connection) =>
                {
                    return connection.QueryFirst<bool>($"SELECT CASE WHEN EXISTS(SELECT * FROM FileStructure_DocumentPath where(workflowId = :workflowId " +
                        " and documentGuid = :documentId)and WorkflowState != 10) THEN 0 ELSE 1 END",
                        new
                        {
                            workflowId, documentId
                        });
                });
            
        }
    }
}
