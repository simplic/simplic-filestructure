using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Data.DB
{
    public class DocumentWorkflowTrackerRepository : SqlRepositoryBase<Guid, DocumentWorkflowTracker>, IDocumentWorkflowTrackerRepository
    {
        private readonly ISqlService sqlService;

        public override string TableName => "IT_Document_Workflow_Tracking";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowTracker obj) => obj.Guid;

        ///<inheritdoc/>
        public bool IsDocumentUserAssigned(Guid documentId, int userId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<bool>($"Select case when Exists(Select * from {TableName} where DocumentId =:documentId " +
                    $"and UserId = :userId) Then 'True' Else 'False' End as Result ",
                    new { documentId, userId }).FirstOrDefault();
            });
        }

        public DocumentWorkflowTrackerRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

    }
}
