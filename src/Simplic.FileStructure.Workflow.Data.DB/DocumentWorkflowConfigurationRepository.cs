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
    /// Represents the repository
    /// </summary>
    public class DocumentWorkflowConfigurationRepository : SqlRepositoryBase<Guid, DocumentWorkflowConfiguration>, IDocumentWorkflowConfigurationRepository
    {
        /// <summary>
        /// Override to get the table name
        /// </summary>
        public override string TableName => "IT_Document_Workflow_Context";

        /// <summary>
        /// Override to get the string 
        /// </summary>
        public override string PrimaryKeyColumn => "Guid";

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="sqlService"> Sql service for database connection pool</param>
        /// <param name="sqlColumnService"> Service for the sql column</param>
        /// <param name="cacheService">Service to set the cache </param>
        public DocumentWorkflowConfigurationRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
        }

        /// <summary>
        /// Gets the id of an object 
        /// </summary>
        /// <param name="obj">Document workflow context</param>
        /// <returns></returns>
        public override Guid GetId(DocumentWorkflowConfiguration obj) => obj.Guid;
    }
}
