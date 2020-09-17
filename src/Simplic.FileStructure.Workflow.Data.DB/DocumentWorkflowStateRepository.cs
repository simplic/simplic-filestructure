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
    /// Represents the document workflow state repository
    /// </summary>
    public class DocumentWorkflowStateRepository : SqlRepositoryBase<Guid, DocumentWorkflowState>, IDocumentWorkflowStateRepository
    {
        private readonly ISqlService sqlService;
        public DocumentWorkflowStateRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }


        public override string TableName => "IT_Document_Workflow_State";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowState obj) => obj.Guid;
    
    }
}
