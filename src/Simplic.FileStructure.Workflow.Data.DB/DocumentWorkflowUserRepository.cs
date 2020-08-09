using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.FileStructure.Workflow;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Data.DB
{
    public class DocumentWorkflowRepository : SqlRepositoryBase<Guid, DocumentWorkflowUser>, IDocumentWorkflowUserRepository
    {
        ISqlService sqlService;
        public DocumentWorkflowRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        public override string TableName => "IT_Document_Workflow";

        public override string PrimaryKeyColumn => "Guid";
        public override Guid GetId(DocumentWorkflowUser obj) => obj.Guid;

        public DocumentWorkflowUser Get(string internalName, int userId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.QueryFirstOrDefault<DocumentWorkflowUser>($"SELECT * FROM {TableName} WHERE InternalName = :internalName AND UserId = :userId",
                    new { internalName, userId });
            });
        }

    }
}
