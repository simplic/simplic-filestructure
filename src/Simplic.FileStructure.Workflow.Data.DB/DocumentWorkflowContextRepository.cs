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
    public class DocumentWorkflowContextRepository : SqlRepositoryBase<Guid, DocumentWorkflowContext>, IDocumentWorkflowContextRepository
    {
        public override string TableName => "";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowContext obj) => obj.Guid;

        private ISqlService sqlService;
        public DocumentWorkflowContextRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

    }
}
