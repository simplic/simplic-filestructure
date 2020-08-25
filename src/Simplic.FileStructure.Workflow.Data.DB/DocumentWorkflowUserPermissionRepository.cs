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
    public class DocumentWorkflowUserPermissionRepository : SqlRepositoryBase<Guid, DocumentWorkflowUserPermission>, IDocumentWorkflowUserPermissionRepository
    {
        ISqlService sqlService;
        public DocumentWorkflowUserPermissionRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        public override string TableName => "IT_Document_Workflow_User_Permission";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowUserPermission obj) => obj.Guid;
    }
}
