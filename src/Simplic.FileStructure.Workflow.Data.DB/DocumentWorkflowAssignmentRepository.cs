﻿using System;
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
    /// <summary>
    /// Represents the document workflow assignment repository 
    /// </summary>
    public class DocumentWorkflowAssignmentRepository : SqlRepositoryBase<Guid, DocumentWorkflowAssignment>, IDocumentWorkflowAssignmentRepository
    {
        private readonly ISqlService sqlService;
        public DocumentWorkflowAssignmentRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
            UseCache = true;
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Table name in the database
        /// </summary>
        public override string TableName => "IT_Document_Workflow_Assignment";

        /// <summary>
        /// Primary key colunm in the database table 
        /// </summary>
        public override string PrimaryKeyColumn => "Guid";

        public bool AlreadyExists(Guid documentId, Guid workflowId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.QueryFirstOrDefault<bool>($"SELECT CASE WHEN EXISTS(" +
                    $"SELECT * FROM {TableName} where DocumentId = :documentId and WorkflowId = :workflowId) " +
                    $"THEN 1 " +
                    $"ELSE 0 END",
                    new { documentId, workflowId });
            });
            
        }

        /// <summary>
        /// Gets the id based on an object 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Guid GetId(DocumentWorkflowAssignment obj) => obj.Guid;
    }
}