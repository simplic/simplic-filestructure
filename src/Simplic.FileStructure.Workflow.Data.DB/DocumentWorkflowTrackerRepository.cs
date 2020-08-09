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
        public override string TableName => "IT_Document_Workflow_Tracking";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowTracker obj) => obj.Guid;

        public DocumentWorkflowTrackerRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
        }

    }
}
