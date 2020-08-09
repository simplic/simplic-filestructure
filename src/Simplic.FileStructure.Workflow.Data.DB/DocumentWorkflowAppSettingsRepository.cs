using Simplic.Data.Sql;
using Simplic.Data;
using Simplic.FileStructure.Workflow;
using Simplic.FileStructure.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Sql;
using Simplic.Cache;

namespace Simplic.FileStructure.Workflow.Data.DB
{
    public class DocumentWorkflowAppSettingsRepository : SqlRepositoryBase<Guid, DocumentWorkflowAppSettings>, IDocumentWorkflowAppSettingsRepository
    {
        ISqlService sqlService;
        public DocumentWorkflowAppSettingsRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }


        public override string TableName => "Application_DocumentWorkflow";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(DocumentWorkflowAppSettings obj) => obj.Guid;
        
    }
}
