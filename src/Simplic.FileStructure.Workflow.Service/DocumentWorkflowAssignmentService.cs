﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// Represents the implementation of the interface <see cref="IDocumentWorkflowAssignmentService"/>
    /// </summary>
    public class DocumentWorkflowAssignmentService : IDocumentWorkflowAssignmentService
    {
        #region[IRepository]

        #endregion
        private readonly IDocumentWorkflowAssignmentRepository repository;
        public DocumentWorkflowAssignmentService(IDocumentWorkflowAssignmentRepository repository)
        {
            this.repository = repository;
        }

        #region[IRepositoryBase]

        public bool Delete(DocumentWorkflowAssignment obj) => repository.Delete(obj);

        public bool Delete(Guid id) => repository.Delete(id);

        public DocumentWorkflowAssignment Get(Guid id) => repository.Get(id);
        

        public IEnumerable<DocumentWorkflowAssignment> GetAll() => repository.GetAll();
        

        public bool Save(DocumentWorkflowAssignment obj) => repository.Save(obj);
        
        #endregion

        public bool AlreadyExists(Guid documentId, Guid workflowId) => repository.AlreadyExists(documentId, workflowId);

    }
}